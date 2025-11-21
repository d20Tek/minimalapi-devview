namespace D20Tek.MinimalApi.DevView.Common;

internal static class DictionaryExtensions
{
    public static string ItemAsString(this Dictionary<string, object?> dict, string key)
    {
        ArgumentNullException.ThrowIfNull(dict);
        return dict.TryGetValue(key, out var value) && value is string result ? result : string.Empty;
    }

    public static string[] ItemAsStringArray(this Dictionary<string, object?> dict, string key)
    {
        ArgumentNullException.ThrowIfNull(dict);
        return dict.TryGetValue(key, out var value) && value is string[] result ? result : [];
    }
}
