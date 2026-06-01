using Umbral.Domain.Entities;
using Umbral.Domain.Exceptions;
using Umbral.Domain.ValueObjects;

namespace Umbral.Domain.Tests.Entities;

public class TriviaTests
{
    private readonly string _validName = "Cultura General";
    private readonly string _validDescription = "Preguntas variadas de cultura general";
    private readonly Guid _validCreatedBy = Guid.NewGuid();

    [Fact]
    public void CreateTrivia_WithValidData_ShouldCreateTrivia()
    {
        // Act
        var trivia = new Trivia(_validName, _validDescription, _validCreatedBy);

        // Assert
        Assert.NotEqual(Guid.Empty, trivia.Id);
        Assert.Equal(_validName, trivia.Name);
        Assert.Equal(_validDescription, trivia.Description);
        Assert.Equal(_validCreatedBy, trivia.CreatedBy);
        Assert.False(trivia.IsDeleted);
        Assert.Empty(trivia.Questions);
    }

    [Fact]
    public void CreateTrivia_WithEmptyName_ShouldThrowException()
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Trivia("", _validDescription, _validCreatedBy));

        Assert.Contains("obligatorio", ex.Message);
    }

    [Fact]
    public void CreateTrivia_WithNameLongerThan100_ShouldThrowException()
    {
        // Arrange
        var longName = new string('a', 101);

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Trivia(longName, _validDescription, _validCreatedBy));

        Assert.Contains("no puede superar", ex.Message);
    }

    [Fact]
    public void CreateTrivia_WithDescriptionLongerThan500_ShouldThrowException()
    {
        // Arrange
        var longDescription = new string('a', 501);

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            new Trivia(_validName, longDescription, _validCreatedBy));

        Assert.Contains("no puede superar", ex.Message);
    }

    [Fact]
    public void UpdateTrivia_WithValidData_ShouldUpdateTrivia()
    {
        // Arrange
        var trivia = new Trivia(_validName, _validDescription, _validCreatedBy);
        var newName = "Ciencia y Tecnología";
        var newDescription = "Preguntas de ciencia y tecnología";

        // Act
        trivia.Update(newName, newDescription);

        // Assert
        Assert.Equal(newName, trivia.Name);
        Assert.Equal(newDescription, trivia.Description);
    }

    [Fact]
    public void AddQuestion_ToTrivia_ShouldAddQuestion()
    {
        // Arrange
        var trivia = new Trivia(_validName, _validDescription, _validCreatedBy);
        var questionText = "¿Cuál es la capital de Francia?";
        var options = new[] { "Berlín", "Madrid", "París", "Lisboa" };
        var correctOption = 2;
        var points = new Points(100);

        // Act
        trivia.AddQuestion(questionText, options, correctOption, points);

        // Assert
        Assert.Single(trivia.Questions);
        var question = trivia.Questions.First();
        Assert.Equal(questionText, question.Text);
        Assert.Equal(options, question.Options);
        Assert.Equal(correctOption, question.CorrectOption);
        Assert.Equal(points, question.Points);
    }

    [Fact]
    public void RemoveQuestion_FromTrivia_ShouldRemoveQuestion()
    {
        // Arrange
        var trivia = new Trivia(_validName, _validDescription, _validCreatedBy);
        trivia.AddQuestion("¿Pregunta?", new[] { "A", "B", "C", "D" }, 0, new Points(100));
        var questionId = trivia.Questions.First().Id;

        // Act
        trivia.RemoveQuestion(questionId);

        // Assert
        Assert.Empty(trivia.Questions);
    }

    [Fact]
    public void RemoveQuestion_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var trivia = new Trivia(_validName, _validDescription, _validCreatedBy);
        var invalidId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => trivia.RemoveQuestion(invalidId));
        Assert.Contains("Pregunta no encontrada", ex.Message);
    }

    [Fact]
    public void UpdateQuestion_ShouldUpdateExistingQuestion()
    {
        // Arrange
        var trivia = new Trivia(_validName, _validDescription, _validCreatedBy);
        trivia.AddQuestion("¿Original?", new[] { "A", "B", "C", "D" }, 0, new Points(100));
        var questionId = trivia.Questions.First().Id;
        var newText = "¿Actualizada?";
        var newOptions = new[] { "X", "Y", "Z", "W" };
        var newCorrectOption = 1;
        var newPoints = new Points(200);

        // Act
        trivia.UpdateQuestion(questionId, newText, newOptions, newCorrectOption, newPoints);

        // Assert
        var question = trivia.Questions.First();
        Assert.Equal(newText, question.Text);
        Assert.Equal(newOptions, question.Options);
        Assert.Equal(newCorrectOption, question.CorrectOption);
        Assert.Equal(newPoints, question.Points);
    }

    [Fact]
    public void Delete_ShouldMarkTriviaAsDeleted()
    {
        // Arrange
        var trivia = new Trivia(_validName, _validDescription, _validCreatedBy);

        // Act
        trivia.Delete();

        // Assert
        Assert.True(trivia.IsDeleted);
    }

    [Fact]
    public void CanBeUsedInSession_WithNoQuestions_ShouldReturnFalse()
    {
        // Arrange
        var trivia = new Trivia(_validName, _validDescription, _validCreatedBy);

        // Act
        var canBeUsed = trivia.CanBeUsedInSession();

        // Assert
        Assert.False(canBeUsed);
    }

    [Fact]
    public void CanBeUsedInSession_WithQuestions_ShouldReturnTrue()
    {
        // Arrange
        var trivia = new Trivia(_validName, _validDescription, _validCreatedBy);
        trivia.AddQuestion("¿Pregunta?", new[] { "A", "B", "C", "D" }, 0, new Points(100));

        // Act
        var canBeUsed = trivia.CanBeUsedInSession();

        // Assert
        Assert.True(canBeUsed);
    }
}