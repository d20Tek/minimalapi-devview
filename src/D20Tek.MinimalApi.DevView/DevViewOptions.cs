namespace D20Tek.MinimalApi.DevView;

public sealed class DevViewOptions
{
    private const string _defaultPath = "/dev";

    public string BasePath { get; set; } = _defaultPath;

    public bool HideDevEndpointsFromOpenApi { get; set; } = true;

    public bool IncludeRequestBodies { get; set; } = false;
    
    public bool IncludeRouteMetadata { get; set; } = true;

    public bool IncludeRouteDebugDetails { get; set; } = false;

    public bool IncludeAllEnvVariables { get; set; } = false;

    public bool ShowSensitiveConfigData { get; set; } = false;
    
    public bool EnableLogging { get; set; } = true;
    
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}
