using Mielek.Expressions.Context;
using Mielek.Model.Attributes;

namespace Mielek.Test.Marshalling;

public class NonStaticExpressionLibrary {

    [Expression]
    public string BodyMethod(IContext context)
    {
        return "BodyMethod";
    }

    [Expression]
    public string ExpressionBodyMethod(IContext context) 
        =>  "ExpressionBodyMethod";
}