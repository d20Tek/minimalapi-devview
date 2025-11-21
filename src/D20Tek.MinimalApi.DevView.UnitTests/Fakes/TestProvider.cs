using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[ExcludeFromCodeCoverage]
internal class TestProvider : IConfigurationProvider
{
    public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string? parentPath) => 
        throw new NotImplementedException();

    public IChangeToken GetReloadToken() => throw new NotImplementedException();
        
    public void Load() => throw new NotImplementedException();
        
    public void Set(string key, string? value) => throw new NotImplementedException();
        
    public bool TryGet(string key, out string? value) => throw new NotImplementedException();
}
