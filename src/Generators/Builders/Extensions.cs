using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BuilderGenerator
{
    public static class Extensions
    {
        public static bool HasAttribute(this SyntaxList<AttributeListSyntax> attributes, string name)
        {
            string fullname, shortname;
            var attrLen = "Attribute".Length;
            if (name.EndsWith("Attribute"))
            {
                fullname = name;
                shortname = name.Remove(name.Length - attrLen, attrLen);
            }
            else
            {
                fullname = name + "Attribute";
                shortname = name;
            }

            return attributes.Any(al => al.Attributes.Any(a => a.Name.ToString().StartsWith(shortname)));
        }
        
        public static T FindParent<T>(this SyntaxNode node) where T : class
        {
            var current = node;
            while(true)
            {
                current = current.Parent;
                if (current == null || current is T)
                    return current as T;
            }
        }
    }
}