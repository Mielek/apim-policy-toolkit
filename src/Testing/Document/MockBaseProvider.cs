﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Testing.Emulator;
using Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Document;

public static class MockBaseProvider
{
    
    public static MockBase Base<T>(this MockPoliciesProvider<T> mock) where T : class => Base(mock, _ => true);

    public static MockBase Base<T>(
        this MockPoliciesProvider<T> mock,
        Func<GatewayContext, bool> predicate
    ) where T : class
    {
        var handler = mock.SectionContextProxy.GetHandler<BaseHandler>();
        return new MockBase(predicate, handler);
    }

    public class MockBase
    {
        private readonly Func<GatewayContext, bool> _predicate;
        private readonly BaseHandler _handler;

        internal MockBase(
            Func<GatewayContext, bool> predicate,
            BaseHandler handler)
        {
            _predicate = predicate;
            _handler = handler;
        }

        public void WithCallback(Action<GatewayContext> callback) =>
            _handler.CallbackHooks.Add(new Tuple<Func<GatewayContext, bool>, Action<GatewayContext>>(_predicate, callback));
    }
}