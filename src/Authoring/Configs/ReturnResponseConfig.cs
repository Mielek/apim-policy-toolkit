namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public record ReturnResponseConfig
{
    public string? ResponseVariableName { get; init; }
    public StatusConfig? Status { get; init; }
    public HeaderConfig[]? Headers { get; init; }
    public BodyConfig? Body { get; init; }
}