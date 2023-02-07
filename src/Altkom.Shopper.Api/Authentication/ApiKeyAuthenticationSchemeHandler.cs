using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Altkom.Shopper.Api.Authentication;

public class ApiKeyAuthenticationSchemeHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
{
    public const string AuthenticationHeaderName = "X-ApiKey";

    public ApiKeyAuthenticationSchemeHandler(
        IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(AuthenticationHeaderName))
        {
            return AuthenticateResult.NoResult();
        }

        var apiKey = Request.Headers[AuthenticationHeaderName];

        if (apiKey != Options.ApiKey)
        {
            return AuthenticateResult.Fail($"Invalid {AuthenticationHeaderName}");
        }

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, "VALID USER")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}

public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; }
}