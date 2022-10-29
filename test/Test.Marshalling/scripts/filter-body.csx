#load "./startup.csx" // Enable intellisense. Will be removed when marshalling
var response = context.Response.Body.As<JObject>();
foreach (var key in new[] { "current", "minutely", "hourly", "daily", "alerts" })
{
    response.Property(key)?.Remove();
};
return response.ToString();