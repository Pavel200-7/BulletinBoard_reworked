using BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Common.SourceInfo;
using System.Collections.Generic;


namespace BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Logging.MethodLogDecorator.SourceInfo
{
    public class PipelineClassInfo
    {
        public ClassInfo ClassInfo { get; }
        public List<PipelineMethodInfo> MethodsInfo { get; }

        public PipelineClassInfo(ClassInfo classInfo, List<PipelineMethodInfo> methodsInfo)
        {
            ClassInfo = classInfo;
            MethodsInfo = methodsInfo;
        }
    }
}
