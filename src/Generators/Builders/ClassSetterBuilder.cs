
using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Attributes;
using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder.Field;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder;

internal class ClassSetterBuilder
{
    private static readonly IFieldSetterHandlerProvider[] _methodProviders = new IFieldSetterHandlerProvider[]
    {
        new ImmutableListFieldHandlerProvider(),
        new ExpressionFieldHandlerProvider(),
        new SimpleFieldHandlerProvider()
    };
    private readonly ClassDeclarationSyntax _classDeclaration;
    private readonly BuilderClassBuilder _classBuilder;

    public ClassSetterBuilder(SemanticModel model, ClassDeclarationSyntax classDeclaration)
    {
        _classDeclaration = classDeclaration;

        var symbol = model.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol ?? throw new Exception("Cannot find symbol model");
        var className = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

        var namespaceName = symbol?.ContainingNamespace.ToDisplayString();
        // namespaceName ??= _classDeclaration.FindParent<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString();
        // namespaceName ??= _classDeclaration.FindParent<NamespaceDeclarationSyntax>()?.Name.ToString();
        if (namespaceName == null) throw new Exception($"Cannot find namespace for class {className}");
        _classBuilder = new BuilderClassBuilder(namespaceName, className);
    }

    public string Build()
    {
        AddUsings();
        AddFieldSetters();
        return _classBuilder.Build();
    }

    private void AddUsings()
    {
        _classBuilder.Using("System.Collections.Immutable");
        _classBuilder.Using("System.Runtime.CompilerServices");
        _classBuilder.Using("Mielek.Azure.ApiManagement.PolicyToolkit.Authoring");
        _classBuilder.Using("Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions");
        _classBuilder.Using("Mielek.Azure.ApiManagement.PolicyToolkit.Builders.Expressions");
    }

    private void AddFieldSetters()
    {
        var fields = _classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>().ToList();
        foreach (var field in fields)
        {
            if (!field.AttributeLists.HasAttribute(nameof(IgnoreBuilderFieldAttribute)))
            {
                AddFieldSetters(field);
            }
        }
    }

    private void AddFieldSetters(FieldDeclarationSyntax field)
    {
        foreach (var provider in _methodProviders)
        {
            if (provider.TryGetHandler(field, out var handler))
            {
                handler.Handle(_classBuilder);
                return;
            }
        }
        throw new Exception();
    }
}