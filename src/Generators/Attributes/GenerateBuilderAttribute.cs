using System;

namespace Mielek.Generator.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateBuilderAttribute : Attribute
    {
        public GenerateBuilderAttribute(Type _)
        { }
    }
}