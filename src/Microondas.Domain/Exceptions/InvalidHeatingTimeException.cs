namespace Microondas.Domain.Exceptions;

public class InvalidHeatingTimeException : Exception
{
    public InvalidHeatingTimeException(string message) : base(message)
    {
    }
}

