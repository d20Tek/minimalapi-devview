namespace D20Tek.MinimalApi.DevView.Common;

internal static class RouteHandlerBuilderExtension
{
    public static IEndpointConventionBuilder WithDevEndpointVisibility(
        this IEndpointConventionBuilder builder,
        bool hideEndpoint) =>
        hideEndpoint ? builder.WithMetadata(new ExcludeFromDescriptionAttribute()) : builder;
}
