using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Microservice.SourceGeneration.Attributes;

namespace Microservice.SourceGeneration.Features
{
    [Generator]
    public class CRUDRequestHandlersGenerator : CustomAttributeSourceGenerator<GenerateCRUDRequestHandlersAttribute>
    {
        protected override void Generate(SourceProductionContext context, INamedTypeSymbol domainModel)
        {
            var handlerClassName = $"{domainModel.Name}CRUDCommandsHandler";
            var sourceBuilder = new StringBuilder($@"
                using {domainModel.ContainingNamespace.ToDisplayString()}
                using MediatR;
                using Microservice.Common.Application.Features;
                using Microservice.Common.Application.Repository;

                namespace Microservice.Generated;
                
                public class {handlerClassName}(IMediator mediator, IGenericRepository<Asset> repository) 
                    : BasicCRUDCommandsHandler<Asset>(mediator, repository)
                {{
                }}
            ");

            context.AddSource($"Microservice.Generated.{handlerClassName}.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}
