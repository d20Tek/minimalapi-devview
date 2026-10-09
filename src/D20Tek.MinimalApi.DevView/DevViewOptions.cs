namespace D20Tek.MinimalApi.DevView;

/// <summary>
/// Configuration options that control the behavior and visibility of the DevView diagnostic endpoints.
/// </summary>
public sealed class DevViewOptions
{
    private const string _defaultPath = "/dev";

    /// <summary>
    /// Gets or sets the base route path under which all DevView endpoints are mapped.
    /// Defaults to "/dev".
    /// </summary>
    public string BasePath { get; set; } = _defaultPath;

    /// <summary>
    /// Gets or sets a value indicating whether DevView endpoints are excluded from OpenAPI/Swagger
    /// documentation. Defaults to <see langword="true"/>.
    /// </summary>
    public bool HideDevEndpointsFromOpenApi { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether request bodies are included in request logging.
    /// Defaults to <see langword="false"/>.
    /// </summary>
    public bool IncludeRequestBodies { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether route metadata is included in the routes explorer
    /// endpoint response. Defaults to <see langword="true"/>.
    /// </summary>
    public bool IncludeRouteMetadata { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether additional route debug details are included in the
    /// routes explorer endpoint response. Defaults to <see langword="false"/>.
    /// </summary>
    public bool IncludeRouteDebugDetails { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether all environment variables are included in the
    /// configuration explorer endpoint response. Defaults to <see langword="false"/>.
    /// </summary>
    public bool IncludeAllEnvVariables { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether sensitive configuration values are shown unmasked
    /// in the configuration explorer endpoint response. Defaults to <see langword="false"/>.
    /// </summary>
    public bool ShowSensitiveConfigData { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether request logging middleware is enabled.
    /// Defaults to <see langword="true"/>.
    /// </summary>
    public bool EnableLogging { get; set; } = true;

    /// <summary>
    /// Gets or sets the <see cref="LogLevel"/> used when logging requests.
    /// Defaults to <see cref="LogLevel.Information"/>.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}
