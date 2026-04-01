using System.Security.Cryptography;
using System.Text;

namespace Microondas.Infrastructure.Services;

public interface IAuthenticationService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
    string GenerateToken(string password);
}

public class AuthenticationService : IAuthenticationService
{
    private const string SecretPassword = "microondas-benner-password";

    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput.Equals(hash);
    }

    public string GenerateToken(string password)
    {
        if (!VerifyPassword(password, HashPassword(SecretPassword)))
            throw new UnauthorizedAccessException("Invalid password");

        var tokenData = $"{SecretPassword}:{DateTime.UtcNow.Ticks}";
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(tokenData));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}

