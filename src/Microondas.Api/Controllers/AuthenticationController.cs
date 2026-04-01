using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microondas.Domain.DTOs;
using Microondas.Infrastructure.Services;

namespace Microondas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] AuthenticationRequest request)
    {
        try
        {
            var token = _authenticationService.GenerateToken(request.Password);
            return Ok(new AuthenticationResponse { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid password" });
        }
    }
}

