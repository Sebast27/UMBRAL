using Umbral.Domain.Exceptions;

namespace Umbral.Domain.ValueObjects;

public record Points
{
    public int Value { get; }

    public Points(int value)
    {
        if (value < 1 || value > 1000)
            throw new DomainException("Los puntos deben estar entre 1 y 1000");

        Value = value;
    }
}