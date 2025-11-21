namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

internal static class TestRouteEndpointFactory
{
    public static List<Dictionary<string, object?>> CreateWithRoute() =>
    [
        new() { { "Pattern", "/v1/test-endpoint" } },
        new() { { "Pattern", "/v2/test-endpoint" } },
        new() { { "Pattern", "/v3/test-endpoint" } }
    ];

    public static List<Dictionary<string, object?>> CreateWithMethod() =>
    [
        new() { { "Method", new string[] { "GET" } } },
        new() { { "Method", new string[] { "POST" } } },
        new() { { "Method", new string[] { "GET" } } }
    ];

    public static List<Dictionary<string, object?>> CreateWithName() =>
    [
        new() { { "Name", "Endpoint1" } },
        new() { { "Name", "Func2" } },
        new() { { "Name", "Endpoint3" } }
    ];

    public static List<Dictionary<string, object?>> CreateWithTags() =>
    [
        new() { { "Tags", new string[] { "foo" } } },
        new() { { "Tags", new string[] { "foo", "bar" } } },
        new() { { "Tags", new string[] { "baz" } } }
    ];

    public static List<Dictionary<string, object?>> CreateWithNullName() =>
    [
        new()
        {
            { "Tags", new string[] { "foo" } },
            { "Name", null }
        },
        new()
        {
            { "Tags", new string[] { "foo", "bar" } },
            { "Name", null }
        },
        new()
        {
            { "Tags", new string[] { "baz" } },
            { "Name", null }
        }
    ];

    public static List<Dictionary<string, object?>> CreateWithNullTags() =>
    [
        new()
        {
            { "Tags", null },
            { "Name", "test1" }
        },
        new()
        {
            { "Tags", null },
            { "Name", "test2" }
        },
        new()
        {
            { "Tags", null },
            { "Name", "test3" }
        }
    ];
}
