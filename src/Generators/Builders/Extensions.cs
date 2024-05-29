using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Generators
{
    public static class Extensions
    {
        public static bool HasAttribute(this SyntaxList<AttributeListSyntax> attributes, string name)
        {
            string fullName, shortname;
            var attrLen = "Attribute".Length;
            if (name.EndsWith("Attribute"))
            {
                fullName = name;
                shortname = name.Remove(name.Length - attrLen, attrLen);
            }
            else
            {
                fullName = name + "Attribute";
                shortname = name;
            }

            return attributes.Any(al => al.Attributes.Any(a => a.IsOfName(shortname)));
        }

        public static bool IsOfName(this AttributeSyntax attribute, string name)
        {
            return attribute.Name.ToString().StartsWith(name);
        }

        public static T? FindParent<T>(this SyntaxNode node) where T : class
        {
            var current = node;
            while (true)
            {
                current = current.Parent;
                if (current == null || current is T)
                    return current as T;
            }
        }
    }
}