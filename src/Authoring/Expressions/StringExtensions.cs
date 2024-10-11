using System.Diagnostics.CodeAnalysis;

using  System.Net.Http.Headers;
using System.Text;


namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring.Expressions;

public static class StringExtensions
{
    public static BasicAuthCredentials? AsBasic(this string value)
    {
        var parts = Encoding.UTF8.GetString(Convert.FromBase64String(value)).Split(':');
        return parts.Length != 2
            ? null
            : new BasicAuthCredentials
            {
                Username = parts[0],
                Password = parts[1]
            };
    }

    public static bool TryParseBasic(this string value, [MaybeNullWhen(false)] out BasicAuthCredentials credentials)
    {
        credentials = value.AsBasic();
        return credentials != null;
    }
    
    public static Jwt? AsJwt(this string value)
    {
        var parts = value.Split('.');
        return parts.Length != 3
            ? null
            : new Jwt
            {
                Algorithm = null,
                Audiences = null,
                Claims = null,
                Id = null,
                Issuer = null,
                Subject = null,
                Type = null
            };
    }

    public static bool TryParseJwt(this string value, [MaybeNullWhen(false)] out Jwt token)
    {
        token = value.AsJwt();
        return token != null;
    }
}