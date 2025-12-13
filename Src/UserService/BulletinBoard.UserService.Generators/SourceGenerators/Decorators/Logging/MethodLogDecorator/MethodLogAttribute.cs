using System;


namespace BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Logging.MethodLogDecorator
{
    public class MethodLogAttribute : Attribute
    {
        public string Message { get; set; }
        public MethodLogAttribute(string message)
        {
            Message = message;
        }
    }
}
