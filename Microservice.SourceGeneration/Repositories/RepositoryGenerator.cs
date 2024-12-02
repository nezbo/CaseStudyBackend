using Microservice.SourceGeneration.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Microservice.SourceGeneration.Repositories
{
    [Generator]
    public class RepositoryGenerator : CustomAttributeSourceGenerator
    {
        protected override string TriggerAttributeName => "GenerateRepository";

        protected override void Generate(SourceProductionContext context, ClassDeclarationSyntax domainModel)
        {
            string modelName = domainModel.GetClassName();
            string className = $"{modelName}Repository";

            var sourceBuilder = new StringBuilder($@"
using {domainModel.GetNamespace()};
using Microservice.Common.Infrastructure.EntityFrameworkCore;
using Microservice.Common.Infrastructure.Repository;

namespace Microservice.Generated {{
    public partial class {className}(IBaseDbContext dbContext) 
    : GenericRepository<{modelName}>(dbContext)
    {{
    }}
}}
            ");

            string sourceStr = sourceBuilder.ToString().Trim();

            context.AddSource($"Microservice.Generated.{className}.g.cs", SourceText.From(sourceStr, Encoding.UTF8));
        }
    }
}
