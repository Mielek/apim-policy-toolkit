// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

public class SectionContextProxy : DispatchProxy
{
    private GatewayContext _context = null!;

    private Dictionary<string, IInvokeHandler> _handlers = null!;

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
            return handler.Invoke(_context, args);
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
    
    public void SetHandler(IInvokeHandler handler)
    {
        _handlers[handler.MethodName] = handler;
    }
    
    private static Dictionary<string, IInvokeHandler> DiscoverHandlers<T>()
    {
        var targetScope = typeof(T).Name;
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type =>
                type is
                {
                    IsClass: true,
                    IsAbstract: false,
                    IsPublic: true,
                    Namespace: "Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Handlers"
                }
                && typeof(IInvokeHandler).IsAssignableFrom(type)
                && type.GetCustomAttributes<SectionAttribute>().Any(att => att.Scope == targetScope))
            .Select(t => Activator.CreateInstance(t) as IInvokeHandler)
            .Where(h => h is not null)
            .ToDictionary(h => h!.MethodName)!;
    }
}