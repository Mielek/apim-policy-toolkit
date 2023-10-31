
using Mielek.Expressions.Context;
using Mielek.Model.Attributes;

namespace Mielek.Test;

[Library]
public class Library
{

    [Expression]
    public static long Test(IContext context)
    {
        return context.Timestamp.Ticks;
    }
}