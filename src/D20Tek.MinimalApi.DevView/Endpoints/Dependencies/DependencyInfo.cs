namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public sealed record DependencyInfo(
    string ServiceType,
    string? Implementation,
    string Lifetime,
    string? AssemblyName);
