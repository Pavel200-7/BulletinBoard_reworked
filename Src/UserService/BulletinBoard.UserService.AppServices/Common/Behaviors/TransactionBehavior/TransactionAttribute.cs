using System.Data;


namespace BulletinBoard.UserService.AppServices.Common.Behaviors.TransactionBehavior;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class TransactionAttribute : Attribute
{
}
