
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Mielek.Generator.Builder.Extensions;

namespace Mielek.Generator.Builder.Field;

public class ExpressionFieldHandlerProvider : IFieldSetterHandlerProvider
{
    public bool TryGetHandler(FieldDeclarationSyntax field, out IFieldSetterHandler handler)
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

        readonly FieldDeclarationSyntax _field;

        public ExpressionFieldHandler(FieldDeclarationSyntax field)
        {
            _field = field;
        }

        public void Handle(BuilderClassBuilder builder)
        {
            foreach (var variable in _field.Declaration.Variables)
            {
                var variableName = variable.Identifier.ToString();
                var methodName = variableName.ToMethodName();
                builder.Method(new BuilderSetMethod(
                    methodName,
                    new[] { "string value" },
                    new[] { $"this.{methodName}(config => config.Constant(value));" }
                ));
                builder.Method(new BuilderSetMethod(
                    methodName, 
                    new[] { "Action<ExpressionBuilder> configurator" },
                    new[] { $"{variableName} = ExpressionBuilder.BuildFromConfiguration(configurator);" }
                ));
            }
        }
    }
}