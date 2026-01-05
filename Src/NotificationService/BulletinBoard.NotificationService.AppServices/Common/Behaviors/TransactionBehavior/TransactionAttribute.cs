namespace BulletinBoard.NotificationService.AppServices.Common.Behaviors.TransactionBehavior;

/// <summary>
/// Атрибут транзакции. Его наложение запускает транзакцию до начала обработки. 
/// Накладывается только на обработчики Mediatr.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class TransactionAttribute : Attribute
{
}