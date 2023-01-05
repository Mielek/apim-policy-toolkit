bool IsVariableSet(IContext context) => context.Variables.ContainsKey("Variable");
string GetKnownGUIDOrGenerateNew(IContext context)
{
    if(!context.Variables.TryGetValue("KnownGUID", out var guid)){
        guid = Guid.NewGuid();
    }
    return guid.ToString();
}