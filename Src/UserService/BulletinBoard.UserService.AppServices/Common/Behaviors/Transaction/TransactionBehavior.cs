using BulletinBoard.UserService.AppServices.Common.IRepository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;


namespace BulletinBoard.UserService.AppServices.Common.Behaviors.Transaction;

/// <summary>
/// Промежуточный запускатор транзакции. 
/// Накладывается аттрибутом TransactionAttribute.
/// </summary>
public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehavior(
        ILogger<TransactionBehavior<TRequest, TResponse>> logger, 
        IServiceProvider serviceProvider, 
        IUnitOfWork unitOfWork)
    {
        _logger = logger;   
        _serviceProvider = serviceProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var handlerType = typeof(IRequestHandler<TRequest, TResponse>);
        var handler = _serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            return await next();
        }

        var attribute = handler.GetType().GetCustomAttribute(typeof(TransactionAttribute));

        if (attribute is null)
        {
            return await next();
        }

        _logger.LogInformation("Начало транзакции.");
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            _logger.LogInformation("Конец транзакции.");
            return response;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogWarning("Транзакция была откачена.");
            throw;
        }
    }
}