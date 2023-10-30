using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Generator.Builder.Field;

public interface IFieldSetterHandlerProvider
{
    bool TryGetHandler(FieldDeclarationSyntax field, [NotNullWhen(true)] out IFieldSetterHandler? handler);
}