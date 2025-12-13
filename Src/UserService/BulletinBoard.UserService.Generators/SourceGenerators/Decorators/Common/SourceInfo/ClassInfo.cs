namespace BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Common.SourceInfo
{
    /// <summary>
    /// Данные класса для кодогенерации.
    /// </summary>
    public class ClassInfo
    {
        public ClassInfo(
            string className,
            string decoratorName,
            string typeNamespace,
            string interfaceName)
        {
            ClassName = className;
            DecoratorName = decoratorName;
            TypeNamespace = typeNamespace;
            InterfaceName = interfaceName;
        }

        public string ClassName { get; }
        public string DecoratorName { get; }
        public string TypeNamespace { get; }
        public string InterfaceName { get; }
    }
}
