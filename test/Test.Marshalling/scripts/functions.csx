string FunctionBodyOneLine(IContext context)
{
    return context.RequestId != null;
}

string FunctionBodyMultiline(IContext context)
{
    var response = context.Response.Body.As<JObject>();
    foreach (var key in new[] { "current", "minutely", "hourly", "daily", "alerts" })
    {
        response.Property(key)?.Remove();
    };
    return response.ToString();
}

string FunctionExpression(IContext context) => context.Elapsed;