namespace Mielek.Azure.ApiManagement.PolicyToolkit.Serialization;

[TestClass]
public class RazorCodeFormatterTests
{
    [TestMethod]
    [DataRow(
        "<element att1=\"@(var a=1;)\" />",
        "<element att1=\"@(var a = 1;)\" />")]
    [DataRow(
        "<element>@(var a=1;)</element>",
        "<element>@(var a = 1;)</element>")]
    [DataRow(
        "<element att1=\"@(context.Request.IpAddress.StartsWith(\"10.0.0.\")?\"a\".ToString():\"b\")\" />",
        "<element att1=\"@(context.Request.IpAddress.StartsWith(\"10.0.0.\") ? \"a\".ToString() : \"b\")\" />")]
    [DataRow(
        "<element>@(context.Request.IpAddress.StartsWith(\"10.0.0.\")?\"a\".ToString():\"b\")</element>",
        "<element>@(context.Request.IpAddress.StartsWith(\"10.0.0.\") ? \"a\".ToString() : \"b\")</element>")]
    public void ShouldFormatOneLineCode(string notFormatted, string formatted)
    {
        var result = RazorCodeFormatter.Format(notFormatted);
        result.Should().Be(formatted);
    }
    
    [TestMethod]
    [DataRow("<element>@{var a=1;}</element>",
        """
        <element>@{
        var a = 1;
        }</element>
        """)]
    [DataRow("<element>@{return context.Request.IpAddress.StartsWith(\"10.0.0.\")?\"a\":\"b\";}</element>", 
        """
        <element>@{
        return context.Request.IpAddress.StartsWith("10.0.0.") ? "a" : "b";
        }</element>
        """)]
    [DataRow(
        """
        <element>@{
        if(context.Request.IpAddress.StartsWith("10.0.0."))
        { return "a"; }
        else
        { return "b"; }
        }</element>
        """,
        """
        <element>@{
        if (context.Request.IpAddress.StartsWith("10.0.0."))
        {
            return "a";
        }
        else
        {
            return "b";
        }
        }</element>
        """)]
    [DataRow(
        """
        <element att1="@{
        if(context.Request.IpAddress.StartsWith("10.0.0."))
        { return "a"; }
        else
        { return "b"; }
        }" />
        """,
        """
        <element att1="@{
        if (context.Request.IpAddress.StartsWith("10.0.0."))
        {
            return "a";
        }
        else
        {
            return "b";
        }
        }" />
        """)]
    public void ShouldFormatMultiLineCode(string notFormatted, string formatted)
    {
        var result = RazorCodeFormatter.Format(notFormatted);
        result.Should().Be(formatted);
    }
}