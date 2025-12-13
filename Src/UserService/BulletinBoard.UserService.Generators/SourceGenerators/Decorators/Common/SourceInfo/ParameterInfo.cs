namespace BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Common.SourceInfo
{
    /// <summary>
    /// Данные параметра метода.
    /// </summary>
    public struct ParameterInfo
    {
        public ParameterInfo(string name, string type, bool isNullable)
        {
            Name = name;
            Type = type;
            IsNullable = isNullable;
        }

        public string Name { get; }
        public string Type { get; }
        public bool IsNullable { get; }
    }
}
