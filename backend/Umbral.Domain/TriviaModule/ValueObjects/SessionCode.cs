using Umbral.Domain.Common.Exceptions;

namespace Umbral.Domain.TriviaModule.ValueObjects;

public record SessionCode
{
    public string Value { get; }

    public SessionCode()
    {
        var random = new Random();
        var chars = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789";
        var code = new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        Value = $"TRIVIA-{code}";
    }

    private SessionCode(string code)
    {
        Value = code;
    }

    public static SessionCode Create(string code) => new(code);

    public override string ToString() => Value;
}