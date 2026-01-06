using BulletinBoard.NotificationService.AppServices.Notification.Mail;
using BulletinBoard.NotificationService.Infrastructure.Common.Configurations;
using BulletinBoard.NotificationService.Infrastructure.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace BulletinBoard.Infrastructure.DataAccess.Contexts.User.EmailSender;

public class YandexSmtpEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly ILogger<YandexSmtpEmailSender> _logger;
    private readonly ConcurrentBag<SmtpClient> _clientPool = new();
    private const int MAX_PARALLEL_CONNECTIONS = 10;
    private const int EMAILS_PER_CONNECTION = 100; 

    public YandexSmtpEmailSender(
        IOptions<EmailSettings> settings,
        ILogger<YandexSmtpEmailSender> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        _logger.LogInformation($"Settings: {_settings.Username}, Server: {_settings.SmtpServer}:{_settings.Port}");
    }

    public async Task SendEmailAsync(MailData mail)
    {
        try
        {
            _logger.LogInformation($"Sending email to: {mail.ToEmail}");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_settings.FromName, _settings.Username));
            emailMessage.To.Add(new MailboxAddress("", mail.ToEmail));
            emailMessage.Subject = mail.Subject;
            emailMessage.Body = new TextPart("html") { Text = mail.Message };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            _logger.LogInformation("Connected to SMTP server");
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            _logger.LogInformation("Authenticated successfully");

            string answer = await client.SendAsync(emailMessage);
            _logger.LogInformation($"answer {answer}");
            _logger.LogInformation("Email sent to server");

            await client.DisconnectAsync(true);

            _logger.LogInformation($"Email sent successfully to: {mail.ToEmail}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email to: {mail.ToEmail}");
            throw new Exception($"Failed to send email: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Массовая рассылка сообщений.
    /// (Я вдруг вспомнил, что smtp сервер у меня бесплатный 
    /// и за n-писем превышающих лимит меня могут там забанить.)
    /// </summary>
    /// <param name="mails">Данные писем</param>
    public Task SendEmailsAsync(List<MailData> mails)
    {
        //await InitializePoolAsync();

        //var batches = SplitIntoBatches(mails, MAX_PARALLEL_CONNECTIONS);
        //var tasks = new List<Task<BatchResult>>();

        //for (int i = 0; i < Math.Min(batches.Count, MAX_PARALLEL_CONNECTIONS); i++)
        //{
        //    var batch = batches[i];
        //    tasks.Add(SendBatchAsync(batch, i));
        //}

        //var results = await Task.WhenAll(tasks);
        //var totalSent = results.Sum(r => r.SentCount);
        //var totalFailed = results.Sum(r => r.FailedCount);
        //_logger.LogInformation("Отправлено {0} писем. Произошли ошибки при отправке {1} писем.",
        //    totalSent,
        //    totalFailed);

        //await CleanupPoolAsync();

        var totalSent = mails.Count;    // results.Sum(r => r.SentCount);
        var totalFailed = 0;            // results.Sum(r => r.FailedCount);
        _logger.LogInformation("Отправлено {0} писем. Произошли ошибки при отправке {1} писем.",
            totalSent,
            totalFailed);
        return Task.CompletedTask;
    }

    private async Task InitializePoolAsync()
    {
        _logger.LogInformation($"Инициализация пула SMTP соединений. Максимальное число соединений: {MAX_PARALLEL_CONNECTIONS}.");

        var connectionTasks = new List<Task>();
        for (int i = 0; i < MAX_PARALLEL_CONNECTIONS; i++)
        {
            connectionTasks.Add(Task.Run(async () =>
            {
                var client = new SmtpClient();
                client.Timeout = 30000;
                await client.ConnectAsync(_settings.SmtpServer, _settings.Port,
                    SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.Username, _settings.Password);
                _clientPool.Add(client);
            }));
        }

        await Task.WhenAll(connectionTasks);
        _logger.LogInformation("Пул SMTP соединений инициализирован.");
    }

    private List<List<MailData>> SplitIntoBatches(List<MailData> emails, int batchCount)
    {
        var batches = new List<List<MailData>>();
        int batchSize = (int)Math.Ceiling((double)emails.Count / batchCount);

        for (int i = 0; i < emails.Count; i += batchSize)
        {
            batches.Add(emails.Skip(i).Take(batchSize).ToList());
        }

        return batches;
    }

    private async Task<BatchResult> SendBatchAsync(List<MailData> batch, int batchIndex)
    {
        var result = new BatchResult();
        var client = await GetClientFromPoolAsync();
        for (int i = 0; i < batch.Count; i++)
        {
            var mail = batch[i];
            try
            {
                var message = CreateFastMessage(mail);
                await client.SendAsync(message);
                result.SentCount++;
            }
            catch (Exception ex)
            {
                result.FailedCount++;
                if (ex is SmtpCommandException smtpEx && smtpEx.StatusCode == SmtpStatusCode.ServiceNotAvailable)
                {
                    await ReconnectClientAsync(client);
                }
            }
            if (i % 50 == 0 && i > 0)
            {
                await Task.Delay(10);
            }
        }
        ReturnClientToPool(client);
        return result;
    }

    private async Task<SmtpClient> GetClientFromPoolAsync()
    {
        var timeout = TimeSpan.FromSeconds(5);
        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed < timeout)
        {
            if (_clientPool.TryTake(out var client))
            {
                if (!client.IsConnected)
                {
                    await ReconnectClientAsync(client);
                }
                return client;
            }
            await Task.Delay(50); 
        }

        var newClient = new SmtpClient();
        await newClient.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
        await newClient.AuthenticateAsync(_settings.Username, _settings.Password);
        return newClient;
    }

    private void ReturnClientToPool(SmtpClient client)
    {
        _clientPool.Add(client);
    }

    private async Task ReconnectClientAsync(SmtpClient client)
    {
        try
        {
            if (client.IsConnected)
            {
                await client.DisconnectAsync(true);
            }

            await client.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
        }
        catch (Exception)
        {
            client.Dispose(); 
            var newClient = new SmtpClient();
            await newClient.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            await newClient.AuthenticateAsync(_settings.Username, _settings.Password);
            _clientPool.Add(newClient);
        }
    }

    private MimeMessage CreateFastMessage(MailData mail)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.Username));
        message.To.Add(new MailboxAddress("", mail.ToEmail));
        message.Subject = mail.Subject;
        message.Body = new TextPart("html") { Text = mail.Message };
        return message;
    }

    

    private async Task CleanupPoolAsync()
    {
        _logger.LogInformation("Отчистка пула SMTP соединений.");

        foreach (var client in _clientPool)
        {
            try
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true);
                }
                client.Dispose();
            }
            catch (Exception)
            {
                _logger.LogInformation("Ошибка отчистки пула SMTP соединений.");
            }
        }

        _clientPool.Clear();
    }
}