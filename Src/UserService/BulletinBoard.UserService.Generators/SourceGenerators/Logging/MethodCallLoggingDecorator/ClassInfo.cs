using System;
using System.Collections.Generic;
using System.Text;

namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator
{
    /// <summary>
    /// Данные слассу для кодогенерации.
    /// </summary>
    public class ClassInfo
    {
        public ClassInfo(string className, string typeNamespace, List<MethodInfo> methodsInfo)
        {
            ClassName = className;
            TypeNamespace = typeNamespace;
            MethodsInfo = methodsInfo;
        }

        public string ClassName { get; }
        public string TypeNamespace { get; }
        public List<MethodInfo> MethodsInfo { get; }
    }
}
