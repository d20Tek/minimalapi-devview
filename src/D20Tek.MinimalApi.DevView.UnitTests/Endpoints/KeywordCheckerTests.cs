using D20Tek.MinimalApi.DevView.Endpoints.Configs;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class KeywordCheckerTests
{
    [TestMethod]
    public void IsSensitive_WithSensitiveKeyword_ReturnsTrue()
    {
        // arrange

        // act
        var result = KeywordChecker.IsSensitive("Secret");

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsSensitive_WithNonSensitiveKeyword_ReturnsFalse()
    {
        // arrange

        // act
        var result = KeywordChecker.IsSensitive("other");

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsSensitive_WithNullKeyword_ReturnsFalse()
    {
        // arrange

        // act
        var result = KeywordChecker.IsSensitive(null);

        // assert
        Assert.IsFalse(result);
    }
}
