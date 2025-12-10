using System.Collections.Generic;


namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator.SourseInfo
{
    /// <summary>
    /// Данные метода для кодогенерации.
    /// </summary>
    public struct MethodInfo
    {
        public MethodInfo(
            string methodName,
            string returnType, 
            bool isAsync, 
            bool isVirtual, 
            bool isOverride, 
            List<ParameterInfo> parameters, 
            string logMessage,
            bool needToBeDecorated)
        {
            MethodName = methodName;
            ReturnType = returnType;
            IsAsync = isAsync;
            IsVirtual = isVirtual;
            IsOverride = isOverride;
            Parameters = parameters;
            LogMessage = logMessage;
            NeedToBeDecorated = needToBeDecorated;
        }

        public string MethodName { get; }
        public string ReturnType { get; }
        public bool IsAsync { get; }
        public bool IsVirtual { get; }
        public bool IsOverride { get; }
        public List<ParameterInfo> Parameters { get; }
        public string LogMessage { get; }
        public bool NeedToBeDecorated { get; }
    }
}
