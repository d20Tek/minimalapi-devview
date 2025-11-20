namespace D20Tek.MinimalApi.DevView.Endpoints.Info;

public sealed record InfoResponse(
    string AppName,
    string Environment,
    string Version,
    DateTime StartTime,
    int UptimeSeconds);
