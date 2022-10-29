#load "./startup.csx"
var response = context.Response.Body.As<JObject>();
foreach (var key in new[] { "current", "minutely", "hourly", "daily", "alerts" })
{
    response.Property(key)?.Remove();
};
return response.ToString();