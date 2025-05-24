namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public static partial class DependenciesEndpoint
{
    internal record DependencyInfo(
        string ServiceType,
        string? Implementation,
        string Lifetime,
        string? AssemblyName);
}
