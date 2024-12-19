// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Testing;
using Azure.ApiManagement.PolicyToolkit.Testing.Document;

namespace Test.Emulator.Emulator.Policies;

[TestClass]
public class AzureOpenAiEmitTokenMetricTests
{
    class SimpleAzureOpenAiEmitTokenMetric : IDocument
    {
        public void Inbound(IInboundContext context)
        {
            context.AzureOpenAiEmitTokenMetric(new EmitTokenMetricConfig
            {
                Dimensions = [new MetricDimensionConfig() { Name = "Test" }]
            });
        }
    }

    [TestMethod]
    public void AzureOpenAiEmitTokenMetric_Callback()
    {
        var test = new SimpleAzureOpenAiEmitTokenMetric().AsTestDocument();
        var executedCallback = false;
        test.MockInbound().AzureOpenAiEmitTokenMetric().WithCallback((_, _) =>
        {
            executedCallback = true;
        });

        test.RunInbound();

        executedCallback.Should().BeTrue();
    }
}