using D20Tek.MinimalApi.DevView.Endpoints.Dependencies;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class ServiceDescriptorExtensionsTests
{
    [TestMethod]
    public void GetCompositeImplementationType_WithNullDescriptor_ThrowsException()
    {
        // arrange
        ServiceDescriptor? descriptor = null;

        // act - assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() =>
            descriptor!.GetCompositeImplementationType());
    }
}
