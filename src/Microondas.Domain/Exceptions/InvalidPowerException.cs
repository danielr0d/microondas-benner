namespace Microondas.Domain.Exceptions;

public class InvalidPowerException : Exception
{
    public InvalidPowerException(string message) : base(message)
    {
    }
}

