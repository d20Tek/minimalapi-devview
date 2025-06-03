using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace D20Tek.MinimalApi.DevView.Endpoints.Configs;

internal sealed class SummaryBuilder
{
    private const string _aspNetUrlsKey = "ASPNETCORE_URLS";
    private const char _urlSeparator = ';';

    private readonly IConfigurationRoot _root;
    private readonly IWebHostEnvironment _hostEnv;
    private readonly IList<ConfigInfo> _configEntries;

    public SummaryBuilder(IConfigurationRoot root, IWebHostEnvironment hostEnv, IList<ConfigInfo> configEntries)
    {
        _root = root;
        _hostEnv = hostEnv;
        _configEntries = configEntries;
    }

    public ConfigSummary Build()
    {
        var loadedJsonFiles = GetLoadedJsonFiles();
        var providerCounts = GetSourceKeyCounts();
        var effectiveUrls = GetEffectiveUrls();

        return new(_hostEnv.EnvironmentName, loadedJsonFiles, providerCounts, effectiveUrls);
    }

    private string[] GetLoadedJsonFiles() =>
        _root.Providers.OfType<JsonConfigurationProvider>()
                       .Select(p => p.Source.Path)
                       .OfType<string>()
                       .Distinct()
                       .ToArray();

    private ProviderInfo[] GetSourceKeyCounts() =>
        _configEntries.GroupBy(e => e.Source)
                      .Select(g => new ProviderInfo(g.Key, g.Count()))
                      .ToArray();

    private string[] GetEffectiveUrls()
    {
        var urls = _root[_aspNetUrlsKey]?.Split(_urlSeparator, StringSplitOptions.RemoveEmptyEntries) ?? [];
        return urls.Select(u => u.Trim()).ToArray();
    }
}
