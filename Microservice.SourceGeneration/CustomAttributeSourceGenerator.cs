using Microservice.SourceGeneration.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservice.SourceGeneration
{
    public abstract class CustomAttributeSourceGenerator
        : IIncrementalGenerator
    {
        protected abstract string TriggerAttributeName { get; }
        protected abstract void Generate(Compilation compilation, SourceProductionContext context, ClassDeclarationSyntax classDecl);

        private static readonly DiagnosticDescriptor _logDescriptor = new(
            id: "SG001",
            title: "Source Generator Log",
            messageFormat: "{0}",
            category: "SourceGenerator",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(context.CompilationProvider, (context, compilation) =>
            {
                var classes = FilterClassesWithAttribute(compilation.SyntaxTrees, this.TriggerAttributeName);

                foreach (var classDecl in classes)
                {
                    this.Generate(compilation, context, classDecl);
                }
            });
        }

        private static IEnumerable<ClassDeclarationSyntax> FilterClassesWithAttribute(IEnumerable<SyntaxTree> trees, string attributeName)
        {
            // Find all class declarations in the syntax tree
            var classDeclarations = trees.SelectMany(t => t.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>());

            // Filter classes that have the specified attribute
            var classesWithAttribute = classDeclarations.Where(classDecl =>
                classDecl.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .Any(attr => attr.Name.ToString() == attributeName));

            return classesWithAttribute;
        }

        protected void Log(SourceProductionContext context, string message) 
            => context.ReportDiagnostic(Diagnostic.Create(_logDescriptor, Location.None, message));

        protected string GetAttributeConstructorArgumentString(Compilation compilation, SourceProductionContext context, ClassDeclarationSyntax classDeclaration, string constructorArgName, int constructorArgIndex, string defaultValue = "")
            => this.GetAttributeConstructorArgument(compilation, context, classDeclaration, constructorArgName, constructorArgIndex)?.Value?.ToString() ?? defaultValue;

        protected TypedConstant? GetAttributeConstructorArgument(Compilation compilation, SourceProductionContext context, ClassDeclarationSyntax classDeclaration, string constructorArgName, int constructorArgIndex)
        {
            // Get the semantic model for the syntax tree
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);

            // Get the symbol for the class declaration
            var classSymbol = semanticModel?.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

            if (classSymbol == null)
            {
                return null;
            }

            // Get the attribute data
            var attributeData = classSymbol.GetAttributes()
                                           .FirstOrDefault(attr => attr.AttributeClass?.Name == this.TriggerAttributeName + "Attribute");

            if (attributeData == null)
            {
                return null;
            }

            // Get named argument
            var namedArgument = attributeData.NamedArguments.FirstOrDefault(kv => kv.Key == constructorArgName);

            if (namedArgument.Key == constructorArgName)
            {
                return namedArgument.Value;
            }

            // Get constructor argument by index
            if (constructorArgIndex < attributeData.ConstructorArguments.Length)
            {
                var constructorArgument = attributeData.ConstructorArguments[constructorArgIndex];
                return constructorArgument;
            }

            return null;
        }
    }
}
