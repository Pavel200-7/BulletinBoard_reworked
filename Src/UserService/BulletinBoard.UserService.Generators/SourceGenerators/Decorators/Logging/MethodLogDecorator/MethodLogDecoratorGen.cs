using BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Common.SourceInfo;
using BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Logging.MethodLogDecorator.SourceInfo;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Logging.MethodLogDecorator
{
    [Generator]
    public class MethodLogDecoratorGen : IIncrementalGenerator
    {
        private string DecoratorName => "MethodLog";
        private string AttributeName => typeof(MethodLogAttribute).Name;
        private string FullAttributeName => typeof(MethodLogAttribute).FullName;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var pipeline = GetPipeLine(context);

            context.RegisterSourceOutput(pipeline, (ctx, pipelineClassInfo) =>
            {
                var decoratorName = $"{pipelineClassInfo.ClassInfo.ClassName}{DecoratorName}Decorator";
                ctx.AddSource($"{decoratorName}.g.cs", SourceText.From(
                    GetDecoratorSourceCodeString(pipelineClassInfo),
                    Encoding.UTF8));
            });

            context.RegisterPostInitializationOutput(ctx =>
            {
                ctx.AddSource("LoggingDecoratorRegistrar.g.cs", SourceText.From(
                    GetDecoratorRegistrarSourceCodeString(),
                    Encoding.UTF8));
            });
        }

        private IncrementalValuesProvider<PipelineClassInfo> GetPipeLine(IncrementalGeneratorInitializationContext context)
        {
            return context.SyntaxProvider
            .CreateSyntaxProvider(
            predicate: (node, cancellationToken) =>
            {
                if (node is not ClassDeclarationSyntax classSyntax)
                    return false;
                if (HasMethodsWithTargetAttribute(classSyntax))
                    return true;
                return false;
            },
            transform: (context, cancellationToken) =>
            {
                var classSyntax = (ClassDeclarationSyntax)context.Node;
                var semanticModel = context.SemanticModel;
                var classSymbol = semanticModel.GetDeclaredSymbol(classSyntax, cancellationToken)
                    as INamedTypeSymbol;


                var attributeType = semanticModel.Compilation
                    .GetTypeByMetadataName(FullAttributeName);

                var methodsInfo = new List<PipelineMethodInfo>();

                foreach (var member in classSymbol.GetMembers().OfType<IMethodSymbol>())
                {
                    if (MethodCanBeSkipedInDecorator(member))
                        continue;

                    MethodInfo methodInfo = GetMethodInfo(member);
                    var attribute = member.GetAttributes()
                        .FirstOrDefault(a =>
                            SymbolEqualityComparer.Default.Equals(
                                a.AttributeClass, attributeType));
                    bool needToBeDecorated = attribute is not null;
                    PipelineAttributeInfo attributeInfo = needToBeDecorated
                        ? GetAttributeInfo(attribute)
                        : new PipelineAttributeInfo();

                    PipelineMethodInfo pipelineMethodInfo = new PipelineMethodInfo(methodInfo, attributeInfo, needToBeDecorated);
                    methodsInfo.Add(pipelineMethodInfo);
                }

                ClassInfo classInfo = GetClassInfo(classSymbol);
                PipelineClassInfo classInfo2 = new PipelineClassInfo(classInfo, methodsInfo);
                return classInfo2;
            })
            .Where(classInfo => classInfo != null)!;
        }

        private bool HasMethodsWithTargetAttribute(ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Members
                .OfType<MethodDeclarationSyntax>()
                .Any(method => method.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .Any(attr =>
                    attr.Name.ToString() == FullAttributeName ||
                    attr.Name.ToString() == AttributeName ||
                    attr.Name.ToString() == DecoratorName));
        }

        private bool MethodCanBeSkipedInDecorator(IMethodSymbol member)
        {
            if (member.MethodKind != MethodKind.Ordinary)
                return true;
            if (member.IsStatic)
                return true;
            if (member.DeclaredAccessibility != Accessibility.Public)
                return true;
            return false;
        }

        private MethodInfo GetMethodInfo(IMethodSymbol member)
        {
            List<ParameterInfo> parameters = member.Parameters.Select(p => new ParameterInfo(
                name: p.Name,
                type: p.Type.ToDisplayString(),
                isNullable: p.NullableAnnotation == NullableAnnotation.Annotated
            )).ToList();

            return new MethodInfo(
                methodName: member.Name,
                returnType: member.ReturnType.ToDisplayString(),
                isAsync: member.IsAsync,
                isVirtual: member.IsVirtual || member.IsOverride,
                isOverride: member.IsOverride,
                parameters: parameters
            );
        }

        private PipelineAttributeInfo GetAttributeInfo(AttributeData attribute)
        {
            string message = "";
            if (attribute.ConstructorArguments.Length > 0)
            {
                if (attribute.ConstructorArguments[0].Value is string constructorMessage)
                {
                    message = constructorMessage;
                }
            }
            return new PipelineAttributeInfo(message);
        }

        private ClassInfo GetClassInfo(INamedTypeSymbol classInfo)
        {
            string decoratorName = $"{classInfo.Name}{DecoratorName}Decorator";
            List<string> interfaces = classInfo.AllInterfaces
                .Select(i => i.ToDisplayString())
                .ToList();
            string mainInterface = interfaces.FirstOrDefault() ??
                                  classInfo.ToDisplayString();

            return new ClassInfo(
               className: classInfo.Name,
               decoratorName: decoratorName,
               typeNamespace: classInfo.ContainingNamespace?.ToDisplayString() ?? "Global",
               interfaceName: mainInterface
           );
        }

        /// <summary>
        /// Выдает строку с кодом для генерации декоратора, который логирует вызов метода.
        /// </summary>
        private string GetDecoratorSourceCodeString(PipelineClassInfo pipelineClassInfo)
        {
            StringBuilder decoratorCode = new StringBuilder();
            decoratorCode.Append(GetTopOfDecoratorModule(pipelineClassInfo.ClassInfo));
            foreach (var methodInfo in pipelineClassInfo.MethodsInfo)
            {
                decoratorCode.Append(GetDecoratorMethodSourceCode(methodInfo));
            }
            decoratorCode.Append(GetBottomOfDecoratorModule());
            return decoratorCode.ToString();
        }

        private string GetTopOfDecoratorModule(ClassInfo classInfo)
        {
            return $@"
using Microsoft.Extensions.Logging;


namespace {classInfo.TypeNamespace}.Decorators

{{
    public class {classInfo.DecoratorName} : {classInfo.InterfaceName}
    {{
        private readonly {classInfo.InterfaceName} _decorated;
        private readonly ILogger<{classInfo.ClassName}> _logger;

        public {classInfo.DecoratorName}(
            {classInfo.InterfaceName} decorated, 
            ILogger<{classInfo.ClassName}> logger)
        {{
            _decorated = decorated;
            _logger = logger;
        }}

            ";
        }

        private string GetDecoratorMethodSourceCode(PipelineMethodInfo pipelineMethodInfo)
        {
            MethodInfo methodInfo = pipelineMethodInfo.MethodInfo;
            string returnType = methodInfo.ReturnType;
            bool isTask = methodInfo.IsAsync ||
                         returnType == "Task" ||
                         returnType.StartsWith("Task<");
            string asyncModifer = isTask ? "async" : "";
            string parameters = string.Join(", ",
                methodInfo.Parameters.Select(p => $"{p.Type} {p.Name}"));
            string arguments = string.Join(", ",
                methodInfo.Parameters.Select(p => p.Name));
            bool isVoid = returnType == "void" || returnType == "System.Void";
            string returnKeyword = isVoid ? "" : "return ";
            string awaitModifer = isTask ? "await" : "";

            PipelineAttributeInfo attributeInfo = pipelineMethodInfo.AttributeInfo;
            var methodData = new MethodSourceData(
                asyncModifer: asyncModifer,
                returnType: returnType,
                methodName: methodInfo.MethodName,
                parameters: parameters,
                logMessage: attributeInfo.Message,
                returnKeyword: returnKeyword,
                awaitModifer: awaitModifer,
                arguments: arguments);

            if (pipelineMethodInfo.NeedToBeDecorated)
                return GetLoggedDecoratorMethodSourceCode(methodData);
            else
                return GetUnloggedDecoratorMethodSourceCode(methodData);
        }

        private string GetLoggedDecoratorMethodSourceCode(MethodSourceData methodData)
        {
            return $@"
            public {methodData.AsyncModifer} {methodData.ReturnType} {methodData.MethodName}({methodData.Parameters})
            {{
                _logger.LogInformation($""{methodData.LogMessage}"");
                try
                {{
                    {methodData.ReturnKeyword} {methodData.AwaitModifer} _decorated.{methodData.MethodName}({methodData.Arguments});
                }}
                catch (Exception ex)
                {{
                    throw;
                }}
            }}
            ";
        }

        private string GetUnloggedDecoratorMethodSourceCode(MethodSourceData methodData)
        {
            return $@"
            public {methodData.AsyncModifer} {methodData.ReturnType} {methodData.MethodName}({methodData.Parameters})
            {{
                try
                {{
                    {methodData.ReturnKeyword} {methodData.AwaitModifer} _decorated.{methodData.MethodName}({methodData.Arguments});
                }}
                catch (Exception ex)
                {{
                    throw;
                }}
            }}
            ";
        }

        private string GetBottomOfDecoratorModule()
        {
            return $@"
    }}
}}
            ";
        }

        private string GetDecoratorRegistrarSourceCodeString()
        {
            return $@"
using System.Reflection;


namespace ESourcerGenerator.Registrar
{{
    public class {DecoratorName}DecoratorRegistrar
    {{
        public static IEnumerable<Type> GetAllDecoratorsType()
        {{
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Name.EndsWith(""{DecoratorName}Decorator"") && !t.IsAbstract);
        }}
    }}
}}
            ";
        }
    }
}
