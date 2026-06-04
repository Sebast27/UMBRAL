using Umbral.Domain.TriviaModule.Entities;
using Umbral.Domain.Common.Exceptions;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Domain.Tests.TriviaModule.Entities;

public class QuestionTests
{
    private readonly string _validText = "¿Cuál es la capital de Francia?";
    private readonly string[] _validOptions = new[] { "Berlín", "Madrid", "París", "Lisboa" };
    private readonly int _validCorrectOption = 2;
    private readonly Points _validPoints = new(100);
    private readonly Guid _validTriviaId = Guid.NewGuid();

    [Fact]
    public void CreateQuestion_WithValidData_ShouldCreateQuestion()
    {
        // Act
        var question = new Question(_validText, _validOptions, _validCorrectOption, _validPoints, _validTriviaId);

        // Assert
        Assert.NotEqual(Guid.Empty, question.Id);
        Assert.Equal(_validText, question.Text);
        Assert.Equal(_validOptions, question.Options);
        Assert.Equal(_validCorrectOption, question.CorrectOption);
        Assert.Equal(_validPoints, question.Points);
        Assert.Equal(_validTriviaId, question.TriviaId);
    }

    [Fact]
    public void CreateQuestion_WithEmptyText_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Question("", _validOptions, _validCorrectOption, _validPoints, _validTriviaId));

        Assert.Contains("obligatorio", ex.Message);
    }

    [Fact]
    public void CreateQuestion_WithTextLongerThan500_ShouldThrowException()
    {
        // Arrange
        var longText = new string('a', 501);

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Question(longText, _validOptions, _validCorrectOption, _validPoints, _validTriviaId));

        Assert.Contains("no puede superar", ex.Message);
    }

    [Fact]
    public void CreateQuestion_WithLessThan4Options_ShouldThrowException()
    {
        // Arrange
        var invalidOptions = new[] { "Opción 1", "Opción 2", "Opción 3" };

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Question(_validText, invalidOptions, _validCorrectOption, _validPoints, _validTriviaId));

        Assert.Contains("exactamente 4 opciones", ex.Message);
    }

    [Fact]
    public void CreateQuestion_WithMoreThan4Options_ShouldThrowException()
    {
        // Arrange
        var invalidOptions = new[] { "Op1", "Op2", "Op3", "Op4", "Op5" };

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Question(_validText, invalidOptions, _validCorrectOption, _validPoints, _validTriviaId));

        Assert.Contains("exactamente 4 opciones", ex.Message);
    }

    [Fact]
    public void CreateQuestion_WithEmptyOption_ShouldThrowException()
    {
        // Arrange
        var invalidOptions = new[] { "Berlín", "", "París", "Lisboa" };

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Question(_validText, invalidOptions, _validCorrectOption, _validPoints, _validTriviaId));

        Assert.Contains("Todas las opciones son obligatorias", ex.Message);
    }

    [Fact]
    public void CreateQuestion_WithDuplicateOptions_ShouldThrowException()
    {
        // Arrange
        var duplicateOptions = new[] { "París", "Madrid", "París", "Lisboa" };

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Question(_validText, duplicateOptions, _validCorrectOption, _validPoints, _validTriviaId));

        Assert.Contains("opciones deben ser únicas", ex.Message);
    }

    [Fact]
    public void CreateQuestion_WithCorrectOptionLessThan0_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Question(_validText, _validOptions, -1, _validPoints, _validTriviaId));

        Assert.Contains("0, 1, 2 o 3", ex.Message);
    }

    [Fact]
    public void CreateQuestion_WithCorrectOptionGreaterThan3_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Question(_validText, _validOptions, 4, _validPoints, _validTriviaId));

        Assert.Contains("0, 1, 2 o 3", ex.Message);
    }

    [Fact]
    public void UpdateQuestion_WithValidData_ShouldUpdateQuestion()
    {
        // Arrange
        var question = new Question(_validText, _validOptions, _validCorrectOption, _validPoints, _validTriviaId);
        var newText = "¿Cuál es la capital de Alemania?";
        var newOptions = new[] { "Berlín", "Madrid", "París", "Lisboa" };
        var newCorrectOption = 0;
        var newPoints = new Points(200);

        // Act
        question.Update(newText, newOptions, newCorrectOption, newPoints);

        // Assert
        Assert.Equal(newText, question.Text);
        Assert.Equal(newOptions, question.Options);
        Assert.Equal(newCorrectOption, question.CorrectOption);
        Assert.Equal(newPoints, question.Points);
    }
}