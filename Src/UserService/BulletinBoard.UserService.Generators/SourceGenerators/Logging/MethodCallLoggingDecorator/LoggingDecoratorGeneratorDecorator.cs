//using System.Linq;
//using System.Reflection;

//namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator
//{
//    public partial class LoggingDecoratorGenerator
//    {
//        /// <summary>
//        /// Выдает строку с кодом для генерации декоратора, который логирует вызов метода.
//        /// </summary>
//        private string GetDecoratorSourceCodeString(MethodInfo info)
//        {
//            var parameters = string.Join(", ", 
//                info.Parameters.Select(p => $"{p.Type} {p.Name}"));

//            var arguments = string.Join(", ",
//                info.Parameters.Select(p => p.Name));

//            var returnType = info.ReturnType;
//            var isTask = info.IsAsync ||
//                         returnType == "Task" ||
//                         returnType.StartsWith("Task<");

//            var methodModifier = info.IsOverride ? "override" :
//                                info.IsVirtual ? "override" : "new";
//            var asyncModifer = isTask ? "async" : "";
//            var awaitModifer = isTask ? "await" : "";


//            return $@"
//using Microsoft.Extensions.Logging;


//namespace {info.TypeNamespace}.Decorators
//{{
//    public class {info.ClassName}{info.MethodName}Decorator : {info.ContainingType}
//    {{
//        private readonly {info.ContainingType} _decorated;
//        private readonly {info.LoggerType} _logger;

//        public {info.ClassName}{info.MethodName}Decorator(
//            {info.ContainingType} decorated, 
//            {info.LoggerType} logger)
//        {{
//            _decorated = decorated;
//            _logger = logger;
//        }}

//        public {methodModifier} {asyncModifer} {returnType} {info.MethodName}({parameters})
//        {{
//            _logger.LogInformation($""[{info.ClassName}{info.MethodName}]   {info.LogMessage}"");
//            try
//            {{
//                return {awaitModifer} _decorated.{info.MethodName}({arguments});
//            }}
//            catch (Exception ex)
//            {{
//                _logger.LogError($""Ошибка обработчика [{info.ClassName}{info.MethodName}]   {{ex.Message}}"");
//                throw;
//            }}

//        }}
//    }}
//}}
//            ";
//        }
//    }
//}
