namespace D20Tek.MinimalApi.DevView.Common;

internal static class FilterExtensions
{
    public static IEnumerable<T> ApplyWhereIf<T>(this IEnumerable<T> source, string? value, Func<T, bool> predicate) =>
        source.ApplyWhereIf(string.IsNullOrWhiteSpace(value) is false, predicate);

    public static IEnumerable<T> ApplyWhereIfEnum<T, TEnum>(
        this IEnumerable<T> source,
        string? value,
        Func<T, TEnum, bool> predicate)
        where TEnum : struct =>
        Enum.TryParse<TEnum>(value, true, out var converted) ? source.Where(x => predicate(x, converted)) : source;

    public static IEnumerable<T> ApplyWhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate) =>
        condition ? source.Where(predicate) : source;
}
