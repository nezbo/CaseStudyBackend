using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Microservice.SourceGeneration
{
    public abstract class CustomAttributeSourceGenerator<TAttribute>
        : IIncrementalGenerator
        where TAttribute : Attribute
    {
        protected abstract void Generate(SourceProductionContext context, INamedTypeSymbol classSymbol);
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Create a provider for class declarations with the custom attribute
            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (s, _) => s is ClassDeclarationSyntax classDecl && classDecl.AttributeLists.Count > 0,
                    transform: (ctx, _) => (ClassDeclarationSyntax)ctx.Node)
                .Where(classDecl => classDecl.AttributeLists
                    .SelectMany(al => al.Attributes)
                        .Any(attr => attr.Name.ToString() == nameof(TAttribute)));

            // Combine the class declarations with the compilation
            var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

            // Register the source output
            context.RegisterSourceOutput(compilationAndClasses, (spc, source) =>
            {
                var (compilation, classes) = source;

                foreach (var classDecl in classes)
                {
                    var model = compilation.GetSemanticModel(classDecl.SyntaxTree);
                    var symbol = model.GetDeclaredSymbol(classDecl);

                    if (symbol.GetAttributes().Any(attr => attr.AttributeClass?.Name == nameof(TAttribute)))
                    {
                        Generate(spc, symbol);
                    }
                }
            });
        }
    }
}
