namespace Microondas.Domain.Exceptions;

public class InvalidHeatingCharacterException : Exception
{
    public InvalidHeatingCharacterException(string message) : base(message)
    {
    }
}

