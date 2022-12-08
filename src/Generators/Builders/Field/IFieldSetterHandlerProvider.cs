using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Generator.Builder.Field;

public interface IFieldSetterHandlerProvider
{
    bool TryGetHandler(FieldDeclarationSyntax field, out IFieldSetterHandler handler);
}