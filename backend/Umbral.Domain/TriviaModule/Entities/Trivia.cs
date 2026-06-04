using Umbral.Domain.Common.Exceptions;
using Umbral.Domain.TriviaModule.ValueObjects;

namespace Umbral.Domain.TriviaModule.Entities;

public class Trivia
{
    private readonly List<Question> _questions = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

    // Constructor para EF Core
    private Trivia() 
    {
        Name = null!;
    }

    // Constructor para crear nueva Trivia
    public Trivia(string name, string? description, Guid createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre de la trivia es obligatorio");

        if (name.Length > 100)
            throw new DomainException("El nombre no puede superar los 100 caracteres");

        if (description?.Length > 500)
            throw new DomainException("La descripción no puede superar los 500 caracteres");

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
        IsDeleted = false;
    }

    // Método para actualizar datos básicos
    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre de la trivia es obligatorio");

        if (name.Length > 100)
            throw new DomainException("El nombre no puede superar los 100 caracteres");

        if (description?.Length > 500)
            throw new DomainException("La descripción no puede superar los 500 caracteres");

        Name = name;
        Description = description;
    }

    // Método para agregar una pregunta (la única forma de crear una Question)
    public void AddQuestion(string text, string[] options, int correctOption, Points points)
    {
        var question = new Question(text, options, correctOption, points, Id);
        _questions.Add(question);
    }

    // Método para actualizar una pregunta existente
    public void UpdateQuestion(Guid questionId, string text, string[] options, int correctOption, Points points)
    {
        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null)
            throw new DomainException("Pregunta no encontrada");

        question.Update(text, options, correctOption, points);
    }

    // Método para eliminar una pregunta
    public void RemoveQuestion(Guid questionId)
    {
        var question = _questions.FirstOrDefault(q => q.Id == questionId);
        if (question is null)
            throw new DomainException("Pregunta no encontrada");

        _questions.Remove(question);
    }

    // Método para eliminar lógicamente la Trivia
    public void Delete()
    {
        IsDeleted = true;
    }

    // Método para verificar si la Trivia se puede usar en una sesión
    public bool CanBeUsedInSession()
    {
        return _questions.Any();
    }
}