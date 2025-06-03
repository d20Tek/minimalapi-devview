namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

internal static class EnvironmentVariables
{
    private static readonly string[] KeyPrefixes =
    [
        "ASPNETCORE_",
        "DOTNET_",
        "WEBSITE_",
        "Logging:",
        "Kestrel:",
        "ConnectionStrings:"
    ];

    public static bool IsRelevant(string key)
    {
        if (string.IsNullOrEmpty(key)) return false;
        return KeyPrefixes.Any(prefix => key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}
