using ESourcerGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging
{
    [Generator]
    public class StudyingGeneraporOfPartitionExtentionMethod : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(ctx =>
            {
                string attrCode = GetAttributeSourseCode();
                ctx.AddSource("MyStudyingAttribute.g.cs",
                    SourceText.From(attrCode, Encoding.UTF8));
            });

            IncrementalValuesProvider<StudyingClassInfo> pipeline = GetPreparedPipeline(context);

            context.RegisterSourceOutput(pipeline, (ctx, classInfo) =>
            {
                string methCode = GetMethodSourseCode(classInfo);
                ctx.AddSource($"{classInfo.Name}.MyStudyingExtention.g.cs",
                    SourceText.From(methCode, Encoding.UTF8));
            });
        }

        public string GetAttributeSourseCode()
        {
            return @"
using System;


namespace ESourcerGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class MyStudyingAttribute : Attribute
    {
        public int augend { get; set; } = 0;
        public int addend { get; set; } = 0; 
    }
}  
            ";
        }

        public IncrementalValuesProvider<StudyingClassInfo> GetPreparedPipeline(IncrementalGeneratorInitializationContext context)
        {
            return context.SyntaxProvider
                .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: "ESourcerGenerator.Attributes.MyStudyingAttribute",
                predicate: (node, cancellationToken) => node is ClassDeclarationSyntax,
                transform: (context, cancellationToken) =>
                {
                    INamedTypeSymbol classSymbol = (INamedTypeSymbol)context.TargetSymbol;
                    AttributeData attribute = context.Attributes.First();

                    int augend = 0;
                    int addend = 0;

                    foreach (var arg in attribute.NamedArguments)
                    {
                        if (arg.Key == "augend" &&
                            arg.Value.Value is int augendVal)
                            augend = augendVal;
                        else if (arg.Key == "addend" &&
                            arg.Value.Value is int addendVal)
                            addend = addendVal;
                    }

                    return new StudyingClassInfo(
                        name: classSymbol.Name,
                        @namespace: classSymbol.ContainingNamespace?.ToDisplayString() ?? "Global",
                        augend: augend,
                        addend: addend
                        );
                });

        }

        public string GetMethodSourseCode(StudyingClassInfo classInfo)
        {
            return $@"
using System;


namespace {classInfo.Namespace}
{{
    public partial class {classInfo.Name} 
    {{
        public int Sum(int addend2)
        {{
            return {classInfo.Augend} + {classInfo.Addend} + addend2;
        }}
    }}
}}
            ";
        }
    }

    public record StudyingClassInfo
    {
        public string Name { get; }
        public string Namespace { get; }
        public int Augend { get; }
        public int Addend { get; }

        public StudyingClassInfo(string name, string @namespace, int augend, int addend)
        {
            Name = name;
            Namespace = @namespace;
            Augend = augend;
            Addend = addend;
        }
    }
}
