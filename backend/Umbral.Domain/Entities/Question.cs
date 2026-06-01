using Umbral.Domain.Exceptions;
using Umbral.Domain.ValueObjects;

namespace Umbral.Domain.Entities;

public class Question
{
    public Guid Id { get; private set; }
    public string Text { get; private set; }
    public string[] Options { get; private set; } = new string[4];
    public int CorrectOption { get; private set; } // 0, 1, 2, 3
    public Points Points { get; private set; }
    public Guid TriviaId { get; private set; }

    // Para EF Core (constructor privado sin parámetros)
    private Question() 
    {
        Text = null!;
        Options = null!;
        Points = null!;
    }

    internal Question(string text, string[] options, int correctOption, Points points, Guid triviaId)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DomainException("El texto de la pregunta es obligatorio");

        if (text.Length > 500)
            throw new DomainException("El texto no puede superar los 500 caracteres");

        if (options.Length != 4)
            throw new DomainException("La pregunta debe tener exactamente 4 opciones");

        for (int i = 0; i < options.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(options[i]))
                throw new DomainException("Todas las opciones son obligatorias");

            if (options[i].Length > 200)
                throw new DomainException("Las opciones no pueden superar los 200 caracteres");
        }

        if (options.Distinct().Count() != options.Length)
            throw new DomainException("Las opciones deben ser únicas");

        if (correctOption < 0 || correctOption > 3)
            throw new DomainException("La opción correcta debe ser 0, 1, 2 o 3");

        Id = Guid.NewGuid();
        Text = text;
        Options = options;
        CorrectOption = correctOption;
        Points = points;
        TriviaId = triviaId;
    }

    internal void Update(string text, string[] options, int correctOption, Points points)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DomainException("El texto de la pregunta es obligatorio");

        if (text.Length > 500)
            throw new DomainException("El texto no puede superar los 500 caracteres");

        if (options.Length != 4)
            throw new DomainException("La pregunta debe tener exactamente 4 opciones");

        for (int i = 0; i < options.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(options[i]))
                throw new DomainException("Todas las opciones son obligatorias");

            if (options[i].Length > 200)
                throw new DomainException("Las opciones no pueden superar los 200 caracteres");
        }

        if (options.Distinct().Count() != options.Length)
            throw new DomainException("Las opciones deben ser únicas");

        if (correctOption < 0 || correctOption > 3)
            throw new DomainException("La opción correcta debe ser 0, 1, 2 o 3");

        Text = text;
        Options = options;
        CorrectOption = correctOption;
        Points = points;
    }
}