// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator;

public class GatewayEmulator
{
    public Dictionary<DocumentScope, IDocument> Documents = new Dictionary<DocumentScope, IDocument>();
    public GatewayContext Context { get; } = new GatewayContext();

    private DocumentScope _documentScope = DocumentScope.Operation;
    
    public void Run()
    {
        SetupBase();
        // try
        // {
        //     Documents[_documentScope].Inbound(Context.InboundContext);
        //     Documents[_documentScope].Backend(Context.BackendContext);
        //     Documents[_documentScope].Outbound(Context.OutboundContext);
        // }
        // catch (PolicyException e)
        // {
        //     _documentScope = DocumentScope.Operation;
        //     Context.LastError.Message = e.Message;
        //     Context.LastError.Section = e.Section;
        //     Context.LastError.PolicyId = e.Policy;
        //     Documents[_documentScope].OnError(Context.OnErrorContext);
        // }
    }

    private void SetupBase()
    {
        // Context.SetHandler<IInboundContext>(new BaseHandler()
        // {
        //     Interceptor = ctx =>
        //     {
        //         SetNextDocument();
        //         Documents[_documentScope].Inbound(ctx.InboundContext);
        //         SetPreviousDocument();
        //     }
        // });
        // Context.SetHandler<IBackendContext>(new BaseHandler()
        // {
        //     Interceptor = ctx =>
        //     {
        //         SetNextDocument();
        //         Documents[_documentScope].Backend(ctx.BackendContext);
        //         SetPreviousDocument();
        //     }
        // });
        // Context.SetHandler<IOutboundContext>(new BaseHandler()
        // {
        //     Interceptor = ctx =>
        //     {
        //         SetNextDocument();
        //         Documents[_documentScope].Outbound(ctx.OutboundContext);
        //         SetPreviousDocument();
        //     }
        // });
        // Context.SetHandler<IOnErrorContext>(new BaseHandler()
        // {
        //     Interceptor = ctx =>
        //     {
        //         SetNextDocument();
        //         Documents[_documentScope].OnError(ctx.OnErrorContext);
        //         SetPreviousDocument();
        //     }
        // });
    }

    void SetNextDocument()
    {
        _documentScope = _documentScope switch
        {
            DocumentScope.Any => throw new InvalidOperationException(),
            DocumentScope.Global => throw new InvalidOperationException(),
            DocumentScope.Workspace => DocumentScope.Global,
            DocumentScope.Product => Documents.ContainsKey(DocumentScope.Workspace)
                ? DocumentScope.Workspace
                : DocumentScope.Global,
            DocumentScope.Api => Documents.ContainsKey(DocumentScope.Product)
                ? DocumentScope.Product
                : Documents.ContainsKey(DocumentScope.Workspace)
                    ? DocumentScope.Workspace
                    : DocumentScope.Global,
            DocumentScope.Operation => DocumentScope.Api,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    void SetPreviousDocument()
    {
        _documentScope = _documentScope switch
        {
            DocumentScope.Any => throw new InvalidOperationException(),
            DocumentScope.Global => Documents.ContainsKey(DocumentScope.Workspace)
                ? DocumentScope.Workspace
                : Documents.ContainsKey(DocumentScope.Product)
                    ? DocumentScope.Product
                    : DocumentScope.Api,
            DocumentScope.Workspace => Documents.ContainsKey(DocumentScope.Product)
                ? DocumentScope.Product
                : DocumentScope.Api,
            DocumentScope.Product => DocumentScope.Api,
            DocumentScope.Api => DocumentScope.Operation,
            DocumentScope.Operation => throw new InvalidOperationException(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
