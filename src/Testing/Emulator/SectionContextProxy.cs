// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

internal class SectionContextProxy : DispatchProxy
{
    private GatewayContext _context = null!;

    private Dictionary<string, IPolicyHandler> _handlers = null!;

    private string _sectionName = null!;

    public static SectionContextProxy Create<T>(GatewayContext expressionContext) where T : class
    {
        var context = (Create(typeof(T), typeof(SectionContextProxy)) as SectionContextProxy)!;
        context._context = expressionContext;
        context._handlers = DiscoverHandlers<T>();
        context._sectionName = typeof(T).Name;
        return context;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);
        ArgumentNullException.ThrowIfNull(targetMethod.DeclaringType);

        if (!_handlers.TryGetValue(targetMethod.Name, out var handler))
        {
            throw new NotImplementedException(targetMethod.Name);
        }

        try
        {
            return handler.Handle(_context, args);
        }
        catch (PolicyException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new PolicyException(e) { Policy = targetMethod.Name, Section = _sectionName };
        }
    }
    
    public void SetHandler(IPolicyHandler handler)
    {
        _handlers[handler.PolicyName] = handler;
    }
    
    internal THandler GetHandler<THandler>() where THandler : class, IPolicyHandler
    {
        var tHandler = Activator.CreateInstance<THandler>();
        if (_handlers.TryGetValue(tHandler.PolicyName, out var handler))
        {
            return handler as THandler ?? throw new InvalidOperationException();
        }

        _handlers[tHandler.PolicyName] = tHandler;
        return tHandler;
    }
    
    private static Dictionary<string, IPolicyHandler> DiscoverHandlers<T>()
    {
        var targetScope = typeof(T).Name;
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type =>
                type is
                {
                    IsClass: true,
                    IsAbstract: false,
                    Namespace: "Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies"
                }
                && typeof(IPolicyHandler).IsAssignableFrom(type)
                && type.GetCustomAttributes<SectionAttribute>().Any(att => att.Scope == targetScope))
            .Select(t => Activator.CreateInstance(t) as IPolicyHandler)
            .Where(h => h is not null)
            .ToDictionary(h => h!.PolicyName)!;
    }
    
}