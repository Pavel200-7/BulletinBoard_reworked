namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator
{
    public partial class LoggingDecoratorGenerator
    {
        /// <summary>
        /// Имя атрибута.
        /// </summary>
        private string AttributeName { get; set; } = "LogCall";

        /// <summary>
        /// Выдает строку с кодом для генерации атрибута, который помечает, что для него нужно определить декоратор.
        /// </summary>
        private string GetAttributeSourceCodeString()
        {
            return $@"
using System;


namespace ESourcerGenerator.Attributes
{{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class {AttributeName}Attribute : Attribute
    {{
        public Type LoggerType {{ get; set; }} = typeof(Microsoft.Extensions.Logging.ILogger);
        public string LogMessage {{ get; set; }}
    }} 
}}
            ";
        }
    }
}
