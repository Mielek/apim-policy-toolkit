using Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

public interface IOutboundContext : IHaveExpressionContext
{
    /// <summary>
    /// Adds header of specified name with values or overrides values if header already exists.
    /// </summary>
    /// <param name="name">
    /// Specifies name of the header to be added. Policy expressions are allowed.
    /// </param>
    /// <param name="values">
    /// Specifies the values of the header to be set. Policy expressions are allowed.
    /// </param>
    void SetHeader(string name, params string[] values);

    /// <summary>
    /// Sets header of specified name and values if header not already exist.
    /// </summary>
    /// <param name="name">
    /// Specifies name of the header to be added. Policy expressions are allowed.
    /// </param>
    /// <param name="values">
    /// Specifies the values of the header to be set. Policy expressions are allowed.
    /// </param>
    void SetHeaderIfNotExist(string name, params string[] values);

    /// <summary>
    /// Adds header of specified name with values or appends values if header already exists.
    /// </summary>
    /// <param name="name">
    /// Specifies name of the header to be added. Policy expressions are allowed.
    /// </param>
    /// <param name="values">
    /// Specifies the values of the header to be set or appended. Policy expressions are allowed.
    /// </param>
    void AppendHeader(string name, params string[] values);

    /// <summary>
    /// Deletes header of specified name.
    /// </summary>
    /// <param name="name">
    /// Specifies name of the header to be deleted. Policy expressions are allowed.
    /// </param>
    void RemoveHeader(string name);

    /// <summary>
    /// The base policy used to specify when parent scope policy should be executed
    /// </summary>
    void Base();

    void SetBody(string body);
}