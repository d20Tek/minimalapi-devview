using Microsoft.Extensions.Logging;

namespace D20Tek.MinimalApi.DevView;

public class DevViewOptions
{
    public string BasePath { get; set; } = "/dev";

    public bool IncludeRequestBodies { get; set; } = false;
    
    public bool IncludeRouteMetadata { get; set; } = true;

    public bool IncludeRouteDebugDetails { get; set; } = false;
    
    public bool EnableLogging { get; set; } = true;
    
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}
