// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Expressions;

public class MockUrl : IUrl
{
    public string Host { get; set; } = "contoso.example";
    public string Path { get; set; } = "/v2/mock/op";
    public string Port { get; set; } = "443";
    public IReadOnlyDictionary<string, string[]> Query => MockQuery;
    public Dictionary<string, string[]> MockQuery { get; set; } = new Dictionary<string, string[]> { { "p1", new[] { "v1" } }, { "p2", new[] { "v1", "v2" } } };
    public string QueryString { get; set; } = "p1=v1&p2=v1&p2=v2";
    public string Scheme { get; set; } = "https";
}