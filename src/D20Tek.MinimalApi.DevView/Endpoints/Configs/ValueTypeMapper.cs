namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

internal static class ValueTypeMapper
{
    public static string? InferFrom(string? value) =>
        value switch
        {
            null => null,
            var v when bool.TryParse(v, out _) => "bool",
            var v when int.TryParse(v, out _) => "int",
            var v when double.TryParse(v, out _) => "double",
            var v when Guid.TryParse(v, out _) => "guid",
            var v when DateTime.TryParse(v, out _) => "datetime",
            _ => "string"
        };
}
