using BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator.SourseInfo;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace BulletinBoard.UserService.Generators.SourceGenerators.Logging.MethodCallLoggingDecorator
{
    public partial class LoggingDecoratorGenerator
    {
        /// <summary>
        /// Выдает провайдер с данными о методах, для которыйх нужен декоратор.
        /// </summary>
        private IncrementalValuesProvider<ClassInfo> GetPipeLine(IncrementalGeneratorInitializationContext context)
        {
            return context.SyntaxProvider
            .CreateSyntaxProvider(
            predicate: (node, cancellationToken) =>
            {
                // Ищем классы
                if (node is not ClassDeclarationSyntax classSyntax)
                    return false;

                // Проверяем, есть ли в классе методы с атрибутом
                return classSyntax.Members
                    .OfType<MethodDeclarationSyntax>()
                    .Any(method => method.AttributeLists
                        .SelectMany(al => al.Attributes)
                        .Any(attr =>
                            attr.Name.ToString() == AttributeName ||
                            attr.Name.ToString() == $"{AttributeName}Attribute"));
            },
            transform: (context, cancellationToken) =>
                {
                    var classSyntax = (ClassDeclarationSyntax)context.Node;
                    var semanticModel = context.SemanticModel;

                    // Приводим к INamedTypeSymbol для доступа к методам
                    var classSymbol = semanticModel.GetDeclaredSymbol(classSyntax, cancellationToken)
                        as INamedTypeSymbol;

                    if (classSymbol == null)
                        return null;

                    // Получаем тип атрибута
                    var logCallAttributeType = semanticModel.Compilation.GetTypeByMetadataName(
                        $"ESourcerGenerator.Attributes.{AttributeName}Attribute");

                    if (logCallAttributeType == null)
                        return null;

                    var methodsInfo = new List<MethodInfo>();
                    string loggerType = "Microsoft.Extensions.Logging.ILogger";

                    // Теперь classSymbol - INamedTypeSymbol и имеет GetMembers()
                    foreach (var member in classSymbol.GetMembers().OfType<IMethodSymbol>())
                    {
                        // Проверяем, является ли метод обычным (не конструктором и т.д.)
                        if (member.MethodKind != MethodKind.Ordinary)
                            continue;

                        // Пропускаем статические методы если нужно
                        if (member.IsStatic)
                            continue;

                        // Проверяем наличие атрибута LogCall
                        var logCallAttribute = member.GetAttributes()
                            .FirstOrDefault(a =>
                                SymbolEqualityComparer.Default.Equals(
                                    a.AttributeClass, logCallAttributeType));

                        bool needToBeDecorated = logCallAttribute != null;
                        string logMessage = "";

                        if (logCallAttribute != null)
                        {
                            // Извлекаем параметры атрибута
                            foreach (var arg in logCallAttribute.NamedArguments)
                            {
                                if (arg.Key == "LoggerType" &&
                                    arg.Value.Value is INamedTypeSymbol typeSymbol)
                                {
                                    loggerType = typeSymbol.ToDisplayString();
                                }
                                else if (arg.Key == "LogMessage" &&
                                            arg.Value.Value is string logMes)
                                {
                                    logMessage = logMes;
                                }
                            }
                        }

                        // Собираем информацию о параметрах
                        var parameters = member.Parameters.Select(p => new ParameterInfo(
                            name: p.Name,
                            type: p.Type.ToDisplayString(),
                            isNullable: p.NullableAnnotation == NullableAnnotation.Annotated
                        )).ToList();

                        var methodInfo = new MethodInfo(
                            methodName: member.Name,
                            returnType: member.ReturnType.ToDisplayString(),
                            isAsync: member.IsAsync,
                            isVirtual: member.IsVirtual || member.IsOverride,
                            isOverride: member.IsOverride,
                            parameters: parameters,
                            logMessage: logMessage,
                            needToBeDecorated: needToBeDecorated
                        );

                        methodsInfo.Add(methodInfo);
                    }

                    // Пропускаем классы без методов для декорации
                    if (!methodsInfo.Any(m => m.NeedToBeDecorated))
                        return null;

                    // Получаем все интерфейсы класса
                    var interfaces = classSymbol.AllInterfaces
                        .Select(i => i.ToDisplayString())
                        .ToList();

                    // Находим "основной" интерфейс (с методами, которые нужно декорировать)
                    string mainInterface = interfaces.FirstOrDefault() ??
                                          classSymbol.ToDisplayString();

                    return new ClassInfo(
                        className: classSymbol.Name,
                        typeNamespace: classSymbol.ContainingNamespace?.ToDisplayString() ?? "Global",
                        interfaceName: mainInterface,
                        loggerType: loggerType,
                        methodsInfo: methodsInfo
                    );
                })
            .Where(classInfo => classInfo != null)!;
        }
    }
}
