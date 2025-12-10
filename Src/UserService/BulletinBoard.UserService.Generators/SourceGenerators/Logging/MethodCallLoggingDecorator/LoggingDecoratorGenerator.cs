using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using System.Text;

namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator
{
    /// <summary>
    /// Создает атрибут, который вешается на метод и логирует начало его выполнения.
    /// </summary>
    [Generator]
    public partial class LoggingDecoratorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                ctx.AddSource($"{AttributeName}.g.cs", SourceText.From(
                    GetAttributeSourceCodeString(),
                    Encoding.UTF8));
            });

            var pipeline = GetPipeLine(context);

            context.RegisterSourceOutput(pipeline, (ctx, classInfo) =>
            {
                var decoratorName = $"{classInfo.InterfaceName}_{classInfo.ClassName}LoggingDecorator";
                ctx.AddSource($"{decoratorName}.g.cs", SourceText.From(
                    GetDecoratorSourceCodeString(classInfo),
                    Encoding.UTF8));
            });
        }
    }
}
