#load "./startup.csx" // Enables intellisense. Will be removed when marshalled
var response = context.Response.Body.As<JObject>();
foreach (var key in new[] { "current", "minutely", "hourly", "daily", "alerts" })
{
    response.Property(key)?.Remove();
};
return response.ToString();