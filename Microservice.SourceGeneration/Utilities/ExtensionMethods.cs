using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microservice.SourceGeneration.Utilities;
internal static class ExtensionMethods
{
    public static string GetClassName(this ClassDeclarationSyntax classDecl) => classDecl.Identifier.Text;

    public static string GetNamespace(this BaseTypeDeclarationSyntax typeDeclaration)
    {
        SyntaxNode? parent = typeDeclaration.Parent;
        while (parent != null)
        {
            if (parent is NamespaceDeclarationSyntax namespaceDeclaration)
            {
                return namespaceDeclaration.Name.ToString();
            }
            else if (parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclaration)
            {
                return fileScopedNamespaceDeclaration.Name.ToString();
            }
            parent = parent.Parent;
        }

        return string.Empty;
    }
}
