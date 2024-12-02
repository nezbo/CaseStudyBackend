using Microservice.SourceGeneration.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Microservice.SourceGeneration.Features
{
    [Generator]
    public class CRUDRequestHandlersGenerator
        : CustomAttributeSourceGenerator
    {
        protected override string TriggerAttributeName => "GenerateCRUDRequestHandlers";

        protected override void Generate(SourceProductionContext context, ClassDeclarationSyntax domainModel)
        {
            var modelName = domainModel.GetClassName();
            var handlerClassName = $"{modelName}CRUDCommandsHandler";
            var sourceBuilder = new StringBuilder($@"
using {domainModel.GetNamespace()};
using MediatR;
using Microservice.Common.Application.Features;
using Microservice.Common.Application.Repository;

namespace Microservice.Generated {{
    public class {handlerClassName} : BasicCRUDCommandsHandler<{modelName}>
    {{
        public {handlerClassName}(IMediator mediator, IGenericRepository<{modelName}> repository) : base(mediator, repository)
        {{
        }}
    }}
}}
            ");

            string sourceStr = sourceBuilder.ToString().Trim();

            context.AddSource($"Microservice.Generated.{handlerClassName}.g.cs", SourceText.From(sourceStr, Encoding.UTF8));
        }
    }
}
