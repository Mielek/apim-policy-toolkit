
using System;
using System.Linq;

using BuilderGenerator;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Generator.Attributes;
using Mielek.Generator.Builder.Field;

namespace Mielek.Generator.Builder;

class ClassSetterBuilder
{
    private static readonly IFieldSetterHandlerProvider[] _methodProviders = new IFieldSetterHandlerProvider[]
    {
        new ListFieldHandlerProvider(),
        new ExpressionFieldHandlerProvider(),
        new SimpleFieldHandlerProvider()
    };

    readonly ClassDeclarationSyntax _classDeclaration;
    readonly BuilderClassBuilder _classBuilder;

    public ClassSetterBuilder(ClassDeclarationSyntax classDeclaration)
    {
        _classDeclaration = classDeclaration;
        var namespaceName = _classDeclaration.FindParent<NamespaceDeclarationSyntax>().Name.ToString();
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