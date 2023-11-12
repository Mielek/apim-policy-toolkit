
using System;
using System.Linq;

using BuilderGenerator;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Generators.Attributes;
using Mielek.Generators.Builder.Field;

namespace Mielek.Generators.Builder;

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

    public ClassSetterBuilder(ClassDeclarationSyntax classDeclaration)
    {
        _classDeclaration = classDeclaration;
        var syntax = _classDeclaration.FindParent<NamespaceDeclarationSyntax>();
        if(syntax == null) throw new Exception();
        var namespaceName = syntax.Name.ToString();
        _classBuilder = new BuilderClassBuilder(namespaceName, classDeclaration.Identifier.Text);
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
        _classBuilder.Using("Mielek.Builders.Expressions");
        _classBuilder.Using("Mielek.Model.Expressions");
        _classBuilder.Using("Mielek.Model.Policies");
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
            if(provider.TryGetHandler(field, out var handler))
            {
                handler.Handle(_classBuilder);
                return;
            }
        }
        throw new Exception();
    }
}