using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using MediatR;
using System.Reflection;


namespace BulletinBoard.UserService.AppServices.Common.Behaviors.TransactionBehavior;

public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehavior(IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
    {
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

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return response;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}