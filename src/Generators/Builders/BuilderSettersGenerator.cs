
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BuilderGenerator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using Mielek.Generator.Attributes;


namespace Mielek.Generator.Builder;

[Generator]
public class BuilderSettersGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var syntaxTree in context.Compilation.SyntaxTrees)
        {
            var classBuilders = GenerateSetterBuilder(context.Compilation, syntaxTree);
            foreach (var classBuilder in classBuilders)
            {
                context.AddSource($"{classBuilder.Key}.Builder.cs", SourceText.From(classBuilder.Value, Encoding.UTF8));
            }
        }
    }

    private Dictionary<string, string> GenerateSetterBuilder(Compilation compilation, SyntaxTree syntaxTree)
    {
        var classToBuilder = new Dictionary<string, string>();

        var root = syntaxTree.GetRoot();
        var usings = (root as CompilationUnitSyntax).Usings.ToString();
        var classesWithAttribute = root
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(cds => cds.AttributeLists.HasAttribute(nameof(GenerateBuilderSettersAttribute)))
            .ToList();

        foreach (var classDeclaration in classesWithAttribute)
        {
            var className = classDeclaration.Identifier.Text;
            classToBuilder[className] = new ClassSetterBuilder(classDeclaration).Build();
        }

        return classToBuilder;
    }

    class ClassSetterBuilder
    {
        readonly ClassDeclarationSyntax _classDeclaration;
        readonly string _className;
        readonly StringBuilder _sourceBuilder;

        public ClassSetterBuilder(ClassDeclarationSyntax classDeclaration)
        {
            _classDeclaration = classDeclaration;
            _className = classDeclaration.Identifier.Text;
            _sourceBuilder = new StringBuilder();
        }

        public string Build()
        {
            AppendUsings();
            AppendNamespace();
            AppendClassStart();
            AppendFieldSetters();
            AppendClassEnd();

            return _sourceBuilder.ToString();
        }

        private void AppendUsings()
        {
            _sourceBuilder.Append("using System.Collections.Immutable;\n");
            _sourceBuilder.Append("using Mielek.Builders.Expressions;\n");
            _sourceBuilder.Append("using Mielek.Model.Expressions;\n");
            _sourceBuilder.Append("using Mielek.Model.Policies;\n");
            _sourceBuilder.Append('\n');
        }

        private void AppendNamespace()
        {
            var namespaceName = _classDeclaration.FindParent<NamespaceDeclarationSyntax>().Name.ToString();
            _sourceBuilder.Append($"namespace {namespaceName};\n");
            _sourceBuilder.Append('\n');
        }

        private void AppendClassStart()
        {
            _sourceBuilder.Append($"public partial class {_className}\n{{\n");
        }

        private void AppendClassEnd()
        {
            _sourceBuilder.Append("\n}");
        }

        private void AppendFieldSetters()
        {
            var fields = _classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>().ToList();
            foreach (var field in fields)
            {
                AppendFieldSetter(field);
            }
        }

        private void AppendFieldSetter(FieldDeclarationSyntax field)
        {
            var type = field.Declaration.Type;
            var strType = type.ToString();
            if (strType.StartsWith("IExpression"))
            {
                foreach (var variable in field.Declaration.Variables)
                {
                    AppendExpressionVariable(variable);
                }
            }
            else if (strType.StartsWith("ImmutableList<IExpression>.Builder"))
            {
                foreach (var variable in field.Declaration.Variables)
                {
                    AppendListExpressionVariable(variable);
                }
            }
            else
            {
                foreach (var variable in field.Declaration.Variables)
                {
                    AppendVariable(type, variable);
                }
            }
        }

        private void AppendListExpressionVariable(VariableDeclaratorSyntax variable)
        {
            var variableName = variable.Identifier.ToString();
            var methodName = ToMethodName(variableName);
            methodName = methodName.Remove(methodName.Length - 1); // remove trailing 's'

            _sourceBuilder.Append($@"
    public {_className} {methodName}(string value)
    {{
        return this.{methodName}(config => config.Constant(value));
    }}

    public {_className} {methodName}(Action<ExpressionBuilder> configurator)
    {{
        var value = ExpressionBuilder.BuildFromConfiguration(configurator);
        ({variableName} ??= ImmutableList.CreateBuilder<IExpression>()).Add(value);
        return this;
    }}
");
        }

        private void AppendExpressionVariable(VariableDeclaratorSyntax variable)
        {
            var variableName = variable.Identifier.ToString();
            var methodName = ToMethodName(variableName);

            _sourceBuilder.Append($@"
    public {_className} {methodName}(string value)
    {{
        return this.{methodName}(config => config.Constant(value));
    }}

    public {_className} {methodName}(Action<ExpressionBuilder> configurator)
    {{
        {variableName} = ExpressionBuilder.BuildFromConfiguration(configurator);
        return this;
    }}
");
        }

        private void AppendVariable(TypeSyntax type, VariableDeclaratorSyntax variable)
        {
            var variableName = variable.Identifier.ToString();
            var methodName = ToMethodName(variableName);
            var paramType = type.ToString().Replace("?", "");
            _sourceBuilder.Append($@"
    public {_className} {methodName}({paramType} value)
    {{
        {variableName} = value;
        return this;
    }}
");
        }

        private string ToMethodName(string variableName)
        {
            var methodName = variableName.Replace("_", "");
            methodName = $"{methodName[0].ToString().ToUpper()}{methodName.Remove(0, 1)}";
            return methodName;
        }
    }



    public void Initialize(GeneratorInitializationContext context)
    {
    }
}