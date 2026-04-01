using Microondas.Infrastructure.Services;
using Microondas.Infrastructure.Repositories;
using Microondas.Domain.Interfaces;
using Microondas.Api.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<Microondas.Infrastructure.Services.IAuthenticationService, Microondas.Infrastructure.Services.AuthenticationService>();
builder.Services.AddScoped<Microondas.Infrastructure.Services.IMicrowaveService, Microondas.Infrastructure.Services.MicrowaveService>();
builder.Services.AddScoped<IProgramRepository, JsonProgramRepository>();

builder.Services.AddAuthentication("Bearer")
    .AddScheme<AuthenticationSchemeOptions, BearerAuthenticationHandler>("Bearer", null);

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();

public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BearerAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.NoResult());

        var authHeader = Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header format"));

        var token = authHeader.Substring("Bearer ".Length).Trim();
        if (string.IsNullOrEmpty(token))
            return Task.FromResult(AuthenticateResult.Fail("Token is empty"));

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[] { new Claim("sub", "user") },
                "Bearer"));

        var ticket = new AuthenticationTicket(principal, "Bearer");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

