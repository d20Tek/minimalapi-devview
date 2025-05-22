using Microsoft.AspNetCore.Http;

namespace D20Tek.MinimalApi.DevView.UnitTests.Fakes;

internal static class HttpContextFactory
{
    public static HttpContext CreateConfiguredRequest()
    {
        var context = new DefaultHttpContext();

        // Configure HttpRequest
        context.Request.Method = "GET";
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("localhost", 5001);
        context.Request.Path = "/api/test";

        return context;
    }
}
