using Microservice.SourceGeneration.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservice.SourceGeneration.Dtos
{
    [Generator]
    public class DtoGenerator : CustomAttributeSourceGenerator
    {
        protected override string TriggerAttributeName => "GenerateDto";

        protected override void Generate(Compilation compilation, SourceProductionContext context, ClassDeclarationSyntax domainModel)
        {
            string modelName = domainModel.GetClassName();
            string className = $"{modelName}Dto";
            string @namespace = this.GetAttributeConstructorArgumentString(compilation, context, domainModel, "@namespace", 0, "Microservice.Generated");

            IEnumerable<string> properties = GeneratePropertyLines(domainModel);

            var sourceBuilder = new StringBuilder($@"
#nullable enable
using Microservice.Common.Presentation.Controllers;
using Microservice.Common.Presentation.Models;

namespace {@namespace} {{
    public partial class {className} : IIdentity, IHasEditUrl
    {{
        public Guid Id {{ get; set; }}

        {string.Join("\r\n\t\t", properties)}

        public string? EditUrl {{ get; set; }}
    }}
}}
            ");

            string sourceStr = sourceBuilder.ToString().Trim();

            context.AddSource($"Microservice.Generated.{className}.g.cs", SourceText.From(sourceStr, Encoding.UTF8));
        }

        private IEnumerable<string> GeneratePropertyLines(ClassDeclarationSyntax domainModel)
        {
            var properties = domainModel.Members
                .AsEnumerable()
                .OfType<PropertyDeclarationSyntax>()
                .Where(p => p.Modifiers.Any(SyntaxKind.PublicKeyword))
                .Where(p => !p.AttributeLists.HasAttribute("GenerateDtoIgnore"))
                .ToList();

            // Do something with the properties
            foreach (var property in properties)
            {
                var propertyName = property.Identifier.Text;
                var propertyType = property.Type.ToString();
                var propertyRequired = property.AttributeLists.HasAttribute("GenerateDtoRequired") ? "required " : "";

                yield return $"public {propertyRequired}{propertyType} {propertyName} {{ get; set; }}";
            }
        }
    }
}
