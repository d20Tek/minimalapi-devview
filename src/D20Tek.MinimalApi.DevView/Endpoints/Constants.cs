namespace D20Tek.MinimalApi.DevView.Endpoints;

internal static class Constants
{
    public const string EndpointTags = "DevView";

    public static class Configurations
    {
        public static string EndpointPattern(string basePath) => $"{basePath}/config";
        public const string EndpointName = "GetDevConfig";
    }

    public static class Dependencies
    {
        public static string EndpointPattern(string basePath) => $"{basePath}/deps";
        public const string EndpointName = "GetDevDependencies";
    }

    public static class Info
    {
        public static string EndpointPattern(string basePath) => $"{basePath}/info";
        public const string EndpointName = "GetDevInfo";

    }

    public static class Routes
    {
        public static string EndpointPattern(string basePath) => $"{basePath}/routes";
        public const string EndpointName = "GetDevRoutes";

    }
}
