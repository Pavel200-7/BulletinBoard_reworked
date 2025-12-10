using System.Collections.Generic;


namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator.SourseInfo
{
    /// <summary>
    /// Данные класса для кодогенерации.
    /// </summary>
    public class ClassInfo
    {
        public ClassInfo(
            string className, 
            string typeNamespace,
            string interfaceName,
            string loggerType, 
            List<MethodInfo> methodsInfo)
        {
            ClassName = className;
            TypeNamespace = typeNamespace;
            InterfaceName = interfaceName;
            LoggerType = loggerType;
            MethodsInfo = methodsInfo;
        }

        public string ClassName { get; }
        public string TypeNamespace { get; }
        public string InterfaceName { get; }
        public string LoggerType { get; }
        public List<MethodInfo> MethodsInfo { get; }
    }
}
