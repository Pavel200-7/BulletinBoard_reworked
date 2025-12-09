using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace ESourcerGenerator
{
    [Generator]
    public class LoggingMethodGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Генерация атрибута
            context.RegisterPostInitializationOutput(ctx => 
            {
                string attributeCode = @"
                using System;


                namespace ESourcerGenerator.Attributes
                {
                    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
                    public sealed class LoggingAttribute : Attribute
                    {
                        public Type LoggerType { get; set; } = typeof(Microsoft.Extensions.Logging.ILogger);
                        public string LogCategory { get; set; } = ""Generated"";
                    }
                }
                ";

                ctx.AddSource("LoggingAttribute.g.cs",
                    SourceText.From(attributeCode, Encoding.UTF8));
            });

            // Поиск классов с атрибутом
            var pipeline = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    fullyQualifiedMetadataName: "ESourcerGenerator.Attributes.LoggingAttribute",
                    predicate: (node, cancellationToken) => node is ClassDeclarationSyntax,
                    transform: (context, cancellationToken) =>
                    {
                        var classSymbol = (INamedTypeSymbol)context.TargetSymbol;
                        var attribute = context.Attributes[0];

                        var loggerType = "Microsoft.Extensions.Logging.ILogger";
                        var logCategory = "Generated";

                        foreach (var arg in attribute.NamedArguments)
                        {
                            if (arg.Key == "LoggerType" &&
                                arg.Value.Value is INamedTypeSymbol typeSymbol)
                                loggerType = typeSymbol.ToDisplayString();
                            else if (arg.Key == "LogCategory")
                                logCategory = arg.Value.Value?.ToString() ?? logCategory;
                        }

                        return new ClassInfo()
                        {
                            Name = classSymbol.Name,
                            Namespace = classSymbol.ContainingNamespace?.ToDisplayString() ?? "Global",
                            LoggerType = loggerType,
                            LogCategory = logCategory
                        };
                    }
            );

            context.RegisterSourceOutput(pipeline, (ctx, classInfo) => 
            {
                string sourceCode = $@"
                using System;
                using Microsoft.Extensions.Logging;

                namespace {classInfo.Namespace}
                {{
                    public partial class {classInfo.Name}
                    {{
                        public void LogMessage(string message)
                        {{
                            _logger.LogInformation(""[{classInfo.Name}] {{Message}}"", message);
                        }}

                        public void PerformAction()
                        {{
                            _logger.LogDebug(""Начало выполнения действия"");
                            // Ваша логика здесь
                            Console.WriteLine(""Действие выполнено!"");
                            _logger.LogInformation(""Действие успешно выполнено"");
                        }}
                    }}
                }}
                ";

                ctx.AddSource($"{classInfo.Name}.LoggingExtensions.g.cs",
                    SourceText.From(sourceCode, Encoding.UTF8));
            });
        }
    }

    public struct ClassInfo
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string LoggerType { get; set; }
        public string LogCategory { get; set; }
    }
}