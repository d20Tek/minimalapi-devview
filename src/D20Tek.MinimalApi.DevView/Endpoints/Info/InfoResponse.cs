namespace D20Tek.MinimalApi.DevView.Endpoints.Info;

public class InfoResponse
{
    public string AppName { get; }
    public string Environment {  get; }
    public string Version { get; }
    public DateTime StartTime { get; }
    public int UptimeSeconds { get; }

    public InfoResponse(string appName, string environment, string version, DateTime startTime, int uptimeSeconds)
    {
        AppName = appName;
        Environment = environment;
        Version = version;
        StartTime = startTime;
        UptimeSeconds = uptimeSeconds;
    }
}
