using System.Collections.Generic;

namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator
{
    /// <summary>
    /// Данные метода для кодогенерации.
    /// </summary>
    public struct MethodInfo
    {
        public MethodInfo(
            string methodName,
            string returnType, 
            string containingType, 
            bool isAsync, 
            bool isVirtual, 
            bool isOverride, 
            List<ParameterInfo> parameters, 
            string loggerType,
            string logMessage,
            bool needToBeDecorated)
        {
            MethodName = methodName;
            ReturnType = returnType;
            ContainingType = containingType;
            IsAsync = isAsync;
            IsVirtual = isVirtual;
            IsOverride = isOverride;
            Parameters = parameters;
            LoggerType = loggerType;
            LogMessage = logMessage;
            NeedToBeDecorated = needToBeDecorated;
        }

        public string MethodName { get; }
        public string ReturnType { get; }
        public string ContainingType { get; }
        public bool IsAsync { get; }
        public bool IsVirtual { get; }
        public bool IsOverride { get; }
        public List<ParameterInfo> Parameters { get; }
        public string LoggerType { get; }
        public string LogMessage { get; }
        public bool NeedToBeDecorated { get; }
    }
}
