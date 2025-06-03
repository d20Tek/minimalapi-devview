namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

internal static class KeywordChecker
{
    private static readonly string[] SensitiveKeywords =
    [
        "password",
        "secret",
        "token",
        "apikey",
        "connectionstring"
    ];

    public static bool IsSensitive(string? key)
    {
        if (string.IsNullOrEmpty(key)) return false;
        return SensitiveKeywords.Any(keyword => key.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
}
