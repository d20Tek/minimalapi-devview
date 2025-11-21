using Microsoft.AspNetCore.Hosting;

namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

internal sealed class SummaryBuilder(IConfigurationRoot root, IWebHostEnvironment hostEnv, IList<ConfigInfo> configEntries)
{
    private const string _aspNetUrlsKey = "ASPNETCORE_URLS";
    private const char _urlSeparator = ';';

    private readonly IConfigurationRoot _root = root;
    private readonly IWebHostEnvironment _hostEnv = hostEnv;
    private readonly IList<ConfigInfo> _configEntries = configEntries;

    public ConfigSummary Build()
    {
        var loadedJsonFiles = GetLoadedJsonFiles();
        var providerCounts = GetSourceKeyCounts();
        var effectiveUrls = GetEffectiveUrls();

        return new(_hostEnv.EnvironmentName, loadedJsonFiles, providerCounts, effectiveUrls);
    }

    private string[] GetLoadedJsonFiles() =>
        [.. _root.Providers.OfType<JsonConfigurationProvider>()
                           .Select(p => p.Source.Path)
                           .OfType<string>()
                           .Distinct()];

    private ProviderInfo[] GetSourceKeyCounts() =>
        [.. _configEntries.GroupBy(e => e.Source).Select(g => new ProviderInfo(g.Key, g.Count()))];

    private string[] GetEffectiveUrls()
    {
        var urls = _root[_aspNetUrlsKey]?.Split(_urlSeparator, StringSplitOptions.RemoveEmptyEntries) ?? [];
        return [.. urls.Select(u => u.Trim())];
    }
}
