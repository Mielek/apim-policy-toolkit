namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

/// <summary>
/// The cors policy adds cross-origin resource sharing (CORS) support to an operation or an API to allow cross-domain calls from browser-based clients.
/// </summary>
public class CorsConfig
{
    /// <summary>
    /// The Access-Control-Allow-Credentials header in the preflight response will be set to the value of this attribute and affect the client's ability to submit credentials in cross-domain requests. Policy expressions are allowed.
    /// </summary>
    public bool? AllowCredentials { get; init; }
    
    /// <summary>
    /// Controls the processing of cross-origin requests that don't match the policy settings. Policy expressions are allowed.
    /// When OPTIONS request is processed as a preflight request and Origin header doesn't match policy settings:
    /// - If the attribute is set to true, immediately terminate the request with an empty 200 OK response
    /// - If the attribute is set to false, check inbound for other in-scope cors policies that are direct children of the inbound element and apply them. If no cors policies are found, terminate the request with an empty 200 OK response.
    /// When GET or HEAD request includes the Origin header (and therefore is processed as a simple cross-origin request), and doesn't match policy settings:
    /// - If the attribute is set to true, immediately terminate the request with an empty 200 OK response.
    /// - If the attribute is set to false, allow the request to proceed normally and don't add CORS headers to the response
    /// </summary>
    public string? TerminateUnmatchedRequest { get; init; }
    
    /// <summary>
    /// Contains origin elements that describe the allowed origins for cross-domain requests. allowed-origins can contain either a single origin element that specifies * to allow any origin, or one or more origin elements that contain a URI.
    /// </summary>
    public required string[] AllowedOrigins { get; init; }
    
    /// <summary>
    /// This element is required if methods other than GET or POST are allowed. Contains method elements that specify the supported HTTP verbs. The value * indicates all methods.
    /// </summary>
    public string[]? AllowedMethods { get; init; }
    
    /// <summary>
    /// The Access-Control-Max-Age header in the preflight response will be set to the value of this attribute and affect the user agent's ability to cache the preflight response. Policy expressions are allowed.
    /// </summary>
    public uint? PreflightResultMaxAge { get; init; }
    
    /// <summary>
    /// This element contains header elements specifying names of the headers that can be included in the request.
    /// </summary>
    public required string[] AllowedHeaders { get; init; }
    
    /// <summary>
    /// This element contains header elements specifying names of the headers that will be accessible by the client.
    /// </summary>
    public string[]? ExposeHeaders { get; init; }
}