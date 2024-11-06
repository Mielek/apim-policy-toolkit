// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Exceptions;

public class PolicyValidationException : Exception
{
    public PolicyValidationException(string message) : base(message)
    {
    }
}