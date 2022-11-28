using Mielek.Model.Expressions;

namespace Mielek.Model.Policies;

public sealed record ChoosePolicy(
    ICollection<ChooseWhen> Whens,
    ICollection<IPolicy>? Otherwise = null
) : Visitable<ChoosePolicy>, IPolicy;
public record ChooseWhen(IExpression Condition, ICollection<IPolicy> Policies);

public sealed record ForwardRequestPolicy(
    uint? Timeout = null,
    bool? FollowRedirects = null,
    bool? BufferRequestBody = null,
    bool? BufferResponse = null,
    bool? FailOnErrorStatusCode = null
) : Visitable<ForwardRequestPolicy>, IPolicy;

public sealed record LimitConcurrencyPolicy(
    IExpression Key,
    uint MaxCount,
    ICollection<IPolicy> Policies
) : Visitable<LimitConcurrencyPolicy>, IPolicy;

public sealed record LogToEventhubPolicy(
    string LoggerId,
    IExpression Value,
    string? PartitionId = null,
    string? PartitionKey = null
) : Visitable<LogToEventhubPolicy>, IPolicy;

public sealed record EmitMetricPolicy(
    string Name,
    ICollection<EmitMetricDimension> Dimensions,
    IExpression? Value = null,
    string? Namespace = null
) : Visitable<EmitMetricPolicy>, IPolicy;
public record EmitMetricDimension(string Name, IExpression? Value = null);

public sealed record MockResponsePolicy(uint? StatusCode = null, string? ContentType = null) : Visitable<MockResponsePolicy>, IPolicy;

public sealed record RecordPolicy(
    IExpression Condition,
    uint Count,
    uint Interval,
    ICollection<IPolicy> Policies,
    uint? MaxInterval = null,
    uint? Delta = null,
    IExpression? FirstFastRetry = null
) : Visitable<RecordPolicy>, IPolicy;

public sealed record ReturnResponsePolicy(
    SetHeaderPolicy? SetHeaderPolicy = null,
    SetBodyPolicy? SetBodyPolicy = null,
    SetStatusPolicy? SetStatusPolicy = null,
    string? ResponseVariableName = null
) : Visitable<ReturnResponsePolicy>, IPolicy;

public sealed record SendOneWayRequestPolicy(
    SendOneWayRequestMode Mode,
    IExpression? SetUrl = null,
    IExpression? Method = null,
    ICollection<SetHeaderPolicy>? SetHeaders = null,
    AuthenticationCertificatePolicy? AuthenticationCertificate = null
) : Visitable<SendOneWayRequestPolicy>, IPolicy;
public enum SendOneWayRequestMode { New, Copy }

public sealed record SendRequestPolicy(
    string ResponseVariableName,
    SendRequestMode? Mode = null,
    uint? Timeout = null,
    bool? IgnoreError = null,
    IExpression? SetUrl = null,
    IExpression? Method = null,
    ICollection<SetHeaderPolicy>? SetHeaders = null,
    AuthenticationCertificatePolicy? AuthenticationCertificate = null
) : Visitable<SendRequestPolicy>, IPolicy;
public enum SendRequestMode { New, Copy }

public sealed record ProxyPolicy(
    string Url,
    string? Username = null,
    string? Password = null
) : Visitable<ProxyPolicy>, IPolicy;

public sealed record SetVariablePolicy(
    string Name,
    IExpression Value
) : Visitable<SetVariablePolicy>, IPolicy;

public sealed record SetMethodPolicy(IExpression Method) : Visitable<SetMethodPolicy>, IPolicy;

public sealed record SetStatusPolicy(IExpression Code, IExpression? Reason = null) : Visitable<SetStatusPolicy>, IPolicy;

public sealed record TracePolicy(
    string Source,
    IExpression Message,
    TraceSeverity? Severity = null,
    ICollection<TraceMetadata>? Metadatas = null
) : Visitable<TracePolicy>, IPolicy;
public enum TraceSeverity {
    Verbose, Information, Error
}
public record TraceMetadata(
    string Name,
    IExpression Value
);

public sealed record WaitPolicy(
    ICollection<IPolicy> Policies,
    WaitFor? For = null
) : Visitable<WaitPolicy>, IPolicy;
public enum WaitFor { All, Any }