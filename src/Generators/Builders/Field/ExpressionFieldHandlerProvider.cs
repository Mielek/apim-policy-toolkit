
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder.Extensions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder.Field;

public class ExpressionFieldHandlerProvider : IFieldSetterHandlerProvider
{
    public bool TryGetHandler(FieldDeclarationSyntax field, [NotNullWhen(true)] out IFieldSetterHandler? handler)
    {
        var type = field.Declaration.Type.ToString();
        if (type.StartsWith("IExpression"))
        {
            handler = new ExpressionFieldHandler(field);
            return true;
        }

        handler = default;
        return false;
    }

    public class ExpressionFieldHandler : IFieldSetterHandler
    {
        private readonly FieldDeclarationSyntax _field;

        public ExpressionFieldHandler(FieldDeclarationSyntax field)
        {
            _field = field;
        }

        public void Handle(BuilderClassBuilder builder)
        {
            var type = _field.Declaration.Type.DescendantNodes()
                .OfType<TypeSyntax>()
                .Last();

            foreach (var variable in _field.Declaration.Variables)
            {
                var variableName = variable.Identifier.ToString();
                var methodName = variableName.ToMethodName();
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { $"{type} value" },
                    new[] { $"{variableName} = ExpressionBuilder<{type}>.Builder.Constant(value).Build();" }
                ));
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { $"Func<IContext, {type}> func, [CallerArgumentExpression(nameof(func))] string? code = null, [CallerFilePath] string? sourceFilePath = null" },
                    new[] { $"{variableName} = ExpressionBuilder<{type}>.Builder.Function(func, code, sourceFilePath).Build();" }
                ));
            }
        }
    }
}