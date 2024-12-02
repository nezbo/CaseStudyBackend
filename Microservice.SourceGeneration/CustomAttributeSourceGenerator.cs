using Microservice.SourceGeneration.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Microservice.SourceGeneration
{
    public abstract class CustomAttributeSourceGenerator
        : IIncrementalGenerator
    {
        protected abstract string TriggerAttributeName { get; }
        protected abstract void Generate(SourceProductionContext context, ClassDeclarationSyntax classDecl);

        private static readonly DiagnosticDescriptor _logDescriptor = new(
            id: "SG001",
            title: "Source Generator Log",
            messageFormat: "{0}",
            category: "SourceGenerator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var classDeclarations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax,
                    transform: static (context, _) => (ClassDeclarationSyntax)context.Node)
                .Where(classDecl => classDecl.AttributeLists
                        .SelectMany(al => al.Attributes)
                            .Any(attr => attr.Name.ToString().Equals(TriggerAttributeName)));

            context.RegisterSourceOutput(classDeclarations, (context, classDecl) =>
            {
                this.Generate(context, classDecl);
            });
        }

        protected void Log(SourceProductionContext context, string message) 
            => context.ReportDiagnostic(Diagnostic.Create(_logDescriptor, Location.None, message));
    }
}
