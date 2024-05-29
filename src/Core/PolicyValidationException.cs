namespace Mielek.Azure.ApiManagement.PolicyToolkit.Exceptions;

public class PolicyValidationException : Exception
{
    public PolicyValidationException(string message) : base(message)
    {
    }
}