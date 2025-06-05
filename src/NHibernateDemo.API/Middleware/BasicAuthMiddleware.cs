using Microsoft.Extensions.Options;
using NHibernateDemo.Infrastructure.Configuration;
using System.Text;

namespace NHibernateDemo.API.Middleware;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BasicAuthMiddleware> _logger;
    private readonly IOptions<BasicAuthOptions> _options;

    public BasicAuthMiddleware(
            RequestDelegate next,
            ILogger<BasicAuthMiddleware> logger,
            IOptions<BasicAuthOptions> options
        )
    {
        _next = next;
        _logger = logger;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string authorizationHeader = context.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("No credentials found in the header.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized.");
            return;
        }

        string base64Credentials = authorizationHeader.Substring("Basic".Length).Trim();

        try
        {
            byte[] decodedBytes = Convert.FromBase64String(base64Credentials);
            string decodedString = Encoding.UTF8.GetString(decodedBytes);
            string[] credentials = decodedString.Split(':');

            if (credentials.Length != 2)
            {
                _logger.LogWarning("Invalid Base64 credentials format.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid Base64 credentials format.");
                return;
            }

            string decodedUsername = credentials[0];
            string decodedPassword = credentials[1];
            string configuredUsername = _options.Value.Username;
            string configuredPassword = _options.Value.Password;

            if (decodedUsername != configuredUsername || decodedPassword != configuredPassword)
            {
                _logger.LogWarning("Invalid credentials provided.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized.");
                return;
            }

            _logger.LogInformation("Authentication successful for user: {Username}", decodedUsername);
            await _next(context);

        }

        catch (ArgumentException ex)
        {
            _logger.LogError("Invalid argument in authentication processing: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Bad request");
            return;
        }
        catch (FormatException ex)
        {
            _logger.LogWarning("Invalid Base64 format in authorization header: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Bad request - Invalid Base64 format");
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error in authentication middleware: {Message}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Internal server error");
            return;
        }
    }
}
