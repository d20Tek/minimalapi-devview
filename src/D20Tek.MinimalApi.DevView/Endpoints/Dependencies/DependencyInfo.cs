namespace D20Tek.MinimalApi.DevView.Endpoints.Dependencies;

public record DependencyInfo(
    string ServiceType,
    string? Implementation,
    string Lifetime,
    string? AssemblyName);
