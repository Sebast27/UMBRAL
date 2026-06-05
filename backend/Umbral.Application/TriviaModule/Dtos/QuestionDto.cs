namespace Umbral.Application.TriviaModule.Dtos;

public record QuestionDto
{
    public Guid Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public string[] Options { get; init; } = Array.Empty<string>();
    public int CorrectOption { get; init; }
    public int Points { get; init; }
}