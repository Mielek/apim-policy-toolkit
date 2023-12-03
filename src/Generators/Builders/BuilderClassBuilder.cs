

using System;
using System.Collections.Generic;
using System.Text;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators.Builder;

public class BuilderClassBuilder
{
    private readonly string _namespaceName;
    private readonly string _className;
    private readonly StringBuilder _sourceBuilder = new StringBuilder();
    private readonly List<string> _usingsTypes = new List<string>();
    private readonly List<BuilderSetMethod> _methods = new List<BuilderSetMethod>();

    public BuilderClassBuilder(string namespaceName, string className)
    {
        _namespaceName = namespaceName;
        _className = className;
    }

    public BuilderClassBuilder Using(string usingType)
    {
        _usingsTypes.Add(usingType);
        return this;
    }

    public BuilderClassBuilder Method(BuilderSetMethod method)
    {
        _methods.Add(method);
        return this;
    }

    public string Build()
    {
        AppendUsings();
        AppendNamespace();
        AppendClassStart();
        AppendMethods();
        AppendClassEnd();
        return _sourceBuilder.ToString();
    }

    private void AppendUsings()
    {
        foreach (var usingType in _usingsTypes)
        {
            _sourceBuilder.Append("using ");
            _sourceBuilder.Append(usingType);
            _sourceBuilder.Append(";\n");
        }
        _sourceBuilder.Append('\n');
    }

    private void AppendNamespace()
    {
        _sourceBuilder.Append("namespace ");
        _sourceBuilder.Append(_namespaceName);
        _sourceBuilder.Append(";\n\n");
    }


    private void AppendClassStart()
    {
        _sourceBuilder.Append("public partial class ");
        _sourceBuilder.Append(_className);
        _sourceBuilder.Append("\n{\n");
    }

    private void AppendMethods()
    {
        foreach (var method in _methods)
        {
            AppendIntend(1);
            _sourceBuilder.Append("public ");
            _sourceBuilder.Append(_className);
            _sourceBuilder.Append(' ');
            _sourceBuilder.Append(method.Name);
            _sourceBuilder.Append('(');

            if (method.Params.Length > 0)
            {
                foreach (var param in method.Params)
                {
                    _sourceBuilder.Append(param);
                    _sourceBuilder.Append(", ");
                }
                _sourceBuilder.Remove(_sourceBuilder.Length - 2, 2); // remove separator
            }

            _sourceBuilder.Append(")\n");
            AppendLineOfCode(1, "{");

            foreach (var line in method.BodyLines)
            {
                AppendLineOfCode(2, line);
            }
            AppendLineOfCode(2, "return this;");
            AppendLineOfCode(1, "}\n");
        }
    }

    private void AppendClassEnd()
    {
        _sourceBuilder.Append("\n}");
    }


    private void AppendIntend(int intend)
    {
        for (int i = 0; i < intend; i++)
        {
            _sourceBuilder.Append("    ");
        }
    }

    private void AppendLineOfCode(int intend, string line)
    {
        AppendIntend(intend);
        _sourceBuilder.Append(line);
        _sourceBuilder.Append('\n');
    }
}

public record BuilderSetMethod(string Name, string[] Params, string[] BodyLines);