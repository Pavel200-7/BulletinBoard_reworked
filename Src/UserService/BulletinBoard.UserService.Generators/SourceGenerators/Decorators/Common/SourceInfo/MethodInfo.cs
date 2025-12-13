using System.Collections.Generic;


namespace BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Common.SourceInfo
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
            List<ParameterInfo> parameters)
        {
            MethodName = methodName;
            ReturnType = returnType;
            IsAsync = isAsync;
            IsVirtual = isVirtual;
            IsOverride = isOverride;
            Parameters = parameters;
        }

        public string MethodName { get; }
        public string ReturnType { get; }
        public bool IsAsync { get; }
        public bool IsVirtual { get; }
        public bool IsOverride { get; }
        public List<ParameterInfo> Parameters { get; }
    }
}
