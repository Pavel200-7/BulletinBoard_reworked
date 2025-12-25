using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.Infrastructure.Middleware.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.Json;


namespace BulletinBoard.UserService.Infrastructure.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, e.Message);
            await HandleNotFoundExceptionAsync(context, e);
        }
        catch (BusinessRuleException e)
        {
            _logger.LogError(e, e.Message);
            await HandleBusinessRuleExceptionAsync(context, e);
        }
        catch (ValidationException e)
        {
            _logger.LogError(e, e.Message);
            await HandleValidationExceptionAsync(context, e);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private async Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status404NotFound;

        var response = new ErrorResponse()
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }

    private async Task HandleBusinessRuleExceptionAsync(HttpContext context, BusinessRuleException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = new FieldsErrorResponse
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = exception.Message,
            FieldFailures = exception.FieldsFailures,
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        var response = new FieldsErrorResponse
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = exception.Message,
            FieldFailures = exception.FieldsFailures,
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new ErrorResponse()
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Message = "Что-то пошло не так. Попробуйте позже.",
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }
}