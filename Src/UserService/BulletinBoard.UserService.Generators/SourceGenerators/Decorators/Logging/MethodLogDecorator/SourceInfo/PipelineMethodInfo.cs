using BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Common.SourceInfo;


namespace BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Logging.MethodLogDecorator.SourceInfo
{
    public class PipelineMethodInfo
    {
        public MethodInfo MethodInfo { get; }
        public PipelineAttributeInfo AttributeInfo { get; }
        public bool NeedToBeDecorated { get; }


        public PipelineMethodInfo(MethodInfo methodInfo, PipelineAttributeInfo attributeInfo, bool needToBeDecorated)
        {
            MethodInfo = methodInfo;
            AttributeInfo = attributeInfo;
            NeedToBeDecorated = needToBeDecorated;
        }
    }
}
