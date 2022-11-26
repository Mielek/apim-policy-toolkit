string IsVariableSet(IContext context) => context.Variables.ContainsKey("Variable").ToString();
string GetKnownGUIDOrGenerateNew(IContext context)
{
    if(!context.Variables.TryGetValue("KnownGUID", out var guid)){
        guid = Guid.NewGuid();
    }
    return guid.ToString();
}