using D20Tek.MinimalApi.DevView.Endpoints.Configs;

namespace D20Tek.MinimalApi.DevView.UnitTests.Endpoints;

[TestClass]
public class ValueTypeMapperTests
{
    [TestMethod]
    [DataRow(null, null)]
    [DataRow("true", "bool")]
    [DataRow("42", "int")]
    [DataRow("1.5", "double")]
    [DataRow("{EEB27864-149A-4595-8C5A-D0E771C50FE3}", "guid")]
    [DataRow("05/03/2025", "datetime")]
    [DataRow("test string", "string")]
    public void InferFrom_WithValue_ReturnsTypeAsString(string? value, string? expected)
    {
        // arrange

        // act
        var result = ValueTypeMapper.InferFrom(value);

        // assert
        Assert.AreEqual(expected, result);
    }
}
