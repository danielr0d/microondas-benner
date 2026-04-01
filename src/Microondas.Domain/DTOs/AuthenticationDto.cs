namespace Microondas.Domain.DTOs;

public class AuthenticationRequest
{
    public string Password { get; set; } = string.Empty;
}

public class AuthenticationResponse
{
    public string Token { get; set; } = string.Empty;
}

