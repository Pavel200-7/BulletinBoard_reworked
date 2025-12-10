using BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator.SourseInfo;
using ESourcerGenerator;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator
{
    public partial class LoggingDecoratorGenerator
    {
        /// <summary>
        /// Выдает строку с кодом для генерации декоратора, который логирует вызов метода.
        /// </summary>
        private string GetDecoratorSourceCodeString(SourseInfo.ClassInfo classInfo)
        {
            StringBuilder decoratorCode = new StringBuilder();
            decoratorCode.Append(GetTopOfModule(classInfo));
            foreach ( var methodInfo in classInfo.MethodsInfo)
            {
                decoratorCode.Append(GetMethodSourceCode(methodInfo));
            }
            decoratorCode.Append(GetBottomOfModule());
            return decoratorCode.ToString();
        }

        private string GetTopOfModule(SourseInfo.ClassInfo classInfo)
        {
            return $@"
using Microsoft.Extensions.Logging;


namespace {classInfo.TypeNamespace}.Decorators

{{
    public class {classInfo.ClassName}LoggingDecorator : {classInfo.InterfaceName}
    {{
        private readonly {classInfo.InterfaceName} _decorated;
        private readonly {classInfo.LoggerType} _logger;

        public {classInfo.ClassName}LoggingDecorator(
            {classInfo.InterfaceName} decorated, 
            {classInfo.LoggerType} logger)
        {{
            _decorated = decorated;
            _logger = logger;
        }}

            ";
        }

        private string GetMethodSourceCode(SourseInfo.MethodInfo methodInfo)
        {
            string returnType = methodInfo.ReturnType;
            bool isTask = methodInfo.IsAsync ||
                         returnType == "Task" ||
                         returnType.StartsWith("Task<");
            string asyncModifer = isTask ? "async" : "";
            string parameters = string.Join(", ",
                methodInfo.Parameters.Select(p => $"{p.Type} {p.Name}"));
            string arguments = string.Join(", ",
                methodInfo.Parameters.Select(p => p.Name));
            bool isVoid = returnType == "void" || returnType == "System.Void";
            string returnKeyword = isVoid ? "" : "return ";
            string awaitModifer = isTask ? "await" : "";

            var methodData = new MethodSourceData(
                asyncModifer: asyncModifer,
                returnType: returnType,
                methodName: methodInfo.MethodName,
                parameters: parameters,
                logMessage: methodInfo.LogMessage,
                returnKeyword: returnKeyword,
                awaitModifer: awaitModifer,
                arguments: arguments);

            if (methodInfo.NeedToBeDecorated)
                return GetLoggedMethodSourceCode(methodData);
            else
                return GetUnloggegMethodSourceCode(methodData);
        }

        private string GetLoggedMethodSourceCode(MethodSourceData methodData)
        {
            return $@"
            public {methodData.AsyncModifer} {methodData.ReturnType} {methodData.MethodName}({methodData.Parameters})
            {{
                _logger.LogInformation($""{methodData.LogMessage}"");
                try
                {{
                    {methodData.ReturnKeyword} {methodData.AwaitModifer} _decorated.{methodData.MethodName}({methodData.Arguments});
                }}
                catch (Exception ex)
                {{
                    throw;
                }}
            }}
            ";
        }

        private string GetUnloggegMethodSourceCode(MethodSourceData methodData)
        {
            return $@"
            public {methodData.AsyncModifer} {methodData.ReturnType} {methodData.MethodName}({methodData.Parameters})
            {{
                try
                {{
                    {methodData.ReturnKeyword} {methodData.AwaitModifer} _decorated.{methodData.MethodName}({methodData.Arguments});
                }}
                catch (Exception ex)
                {{
                    throw;
                }}
            }}
            ";
        }

        private string GetBottomOfModule()
        {
            return $@"
    }}
}}
            ";
        }
    }
}
