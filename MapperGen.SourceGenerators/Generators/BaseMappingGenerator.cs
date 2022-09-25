using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace MapperGen.SourceGenerators.Generators;

[Generator(LanguageNames.CSharp)]
public class BaseMappingGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classesToDtos = context.SyntaxProvider.CreateSyntaxProvider(NeedsDto, GetClassOrNull)
            .Where(currentType => currentType is not null)
            .Collect();

        context.RegisterSourceOutput(classesToDtos, GenerateCode);
    }

    private void GenerateCode(SourceProductionContext ctx, ImmutableArray<ITypeSymbol> classes)
    {
        if (classes.IsDefaultOrEmpty)
            return;

        StringBuilder sb = new StringBuilder();

        foreach (var item in classes)
        {

            sb.AppendLine(
                $$"""
                namespace Generator.{{item}}.Dto
                {
                    public class {{item.Name}}Dto
                    {
                """);
            foreach (var prop in item.GetMembers().OfType<IPropertySymbol>())
            {
                sb.AppendLine(
                    $$"""
                            public {{prop.Type.Name}} {{prop.Name}} { get; set; }
                    """);
            }

            sb.Append(
                $$"""
                     } 
                 }
                 """);
            sb.Append("\n");


        }
        ctx.AddSource($"{classes[0].ContainingNamespace}GeneratedClasses.g.cs", sb.ToString());
    }

    private bool NeedsDto(SyntaxNode stxNode, CancellationToken ct)
    {
        if (stxNode is not AttributeSyntax attribute)
            return false;

        var name = ExtractCurrentName(attribute.Name);

        return name is "GenerateMappedDto" or "GenerateMappedDtoAttribute";
    }

    private string ExtractCurrentName(NameSyntax attributeName)
    {
        while (attributeName is not null)
        {
            switch (attributeName)
            {
                case IdentifierNameSyntax ins:
                    return ins.Identifier.Text;
                case QualifiedNameSyntax qns:
                    attributeName = qns.Right;

                    break;
                default:
                    return null;
            }
        }

        return null;
    }

    private ITypeSymbol GetClassOrNull(GeneratorSyntaxContext ctx, CancellationToken ct)
    {
        var attrStx = (AttributeSyntax)ctx.Node;

        if (attrStx.Parent?.Parent is not ClassDeclarationSyntax classDecl)
            return null;

        var currentType = ctx.SemanticModel.GetDeclaredSymbol(classDecl) as ITypeSymbol;

        var current = currentType is null || !NeedMap(currentType) ? null : currentType;

        return current;
    }

    private bool NeedMap(ISymbol currentType)
    {
        var current = currentType.GetAttributes().Any(x => x.AttributeClass?.Name == "GenerateMappedDtoAttribute");

        return current;
    }
}

