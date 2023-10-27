using Mielek.Expressions.Context;
using Mielek.Model.Attributes;

namespace Mielek.Test.Marshalling;

public static class StaticExpressionLibrary {

    [Expression]
    public static string StaticBodyMethod(IContext context)
    {
        return "StaticBodyMethod";
    }

    [Expression]
    public static string StaticExpressionBodyMethod(IContext context) 
        =>  "StaticExpressionBodyMethod";
}