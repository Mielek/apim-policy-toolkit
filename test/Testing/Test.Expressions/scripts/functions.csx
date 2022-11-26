string FunctionBodyOneLine(IContext context)
{
    return context.RequestId.ToString();
}

string FunctionBodyMultiline(IContext context)
{
    string someLine = "test";
    string concatString = someLine + context.RequestId.ToString();
    string requestId = concatString.Remove(0, someLine.Length);
    return requestId;
}

string FunctionExpression(IContext context) => context.RequestId.ToString();