using Umbral.Domain.Common.Exceptions;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Domain.Tests.TriviaModule.ValueObjects;

public class PointsTests
{
    [Fact]
    public void CreatePoints_WithValidValue_ShouldCreatePoints()
    {
        // Arrange
        var value = 100;

        // Act
        var points = new Points(value);

        // Assert
        Assert.Equal(value, points.Value);
    }

    [Fact]
    public void CreatePoints_WithValueLessThan1_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => new Points(0));
        Assert.Contains("entre 1 y 1000", ex.Message);
    }

    [Fact]
    public void CreatePoints_WithValueGreaterThan1000_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => new Points(1001));
        Assert.Contains("entre 1 y 1000", ex.Message);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(500)]
    [InlineData(1000)]
    public void CreatePoints_WithValidValues_ShouldCreatePoints(int value)
    {
        // Act
        var points = new Points(value);

        // Assert
        Assert.Equal(value, points.Value);
    }
}