namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator.SourseInfo
{
    /// <summary>
    /// Частная информация о методе, необходимая для вставки в код сорсгенератора.
    /// </summary>
    public class MethodSourceData
    {
        public MethodSourceData(
            string asyncModifer, 
            string returnType, 
            string methodName, 
            string parameters, 
            string logMessage,
            string returnKeyword,
            string awaitModifer, 
            string arguments)
        {
            AsyncModifer = asyncModifer;
            ReturnType = returnType;
            MethodName = methodName;
            Parameters = parameters;
            LogMessage = logMessage;
            ReturnKeyword = returnKeyword;
            AwaitModifer = awaitModifer;
            Arguments = arguments;
        }

        public string AsyncModifer { get; }
        public string ReturnType { get; }
        public string MethodName { get; }
        public string Parameters { get; }
        public string LogMessage { get; }
        public string ReturnKeyword { get; }
        public string AwaitModifer { get; }
        public string Arguments { get; }
    }
}
