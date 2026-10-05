namespace EkubCircle.Domain.ValueObjects;

public record Money(decimal Amount, string Currency = "ETB")
{
    public static Money Zero => new(0m, "ETB");
}