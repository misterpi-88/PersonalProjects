using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using ToDo.Application.Common.Options;

namespace ToDo.Application.Common.Handlers;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IOptionsMonitor<ApiOptions> _apiOptions;
    public const string SchemeName = "ApiKey";

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        IOptionsMonitor<ApiOptions> apiOptions,
        ILoggerFactory loggerFactory,
        UrlEncoder urlEncoder)
        : base(options, loggerFactory, urlEncoder)
    {
        _apiOptions = apiOptions;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("api-key", out var apiKeyHeaderValue))
        {
            return Task.FromResult(AuthenticateResult.Fail("The API key is missing from the request header"));
        }

        if (apiKeyHeaderValue != _apiOptions.CurrentValue.Key)
        {
            return Task.FromResult(AuthenticateResult.Fail("The API key in the request header does not match the config"));
        }

        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, SchemeName)], SchemeName);

        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName)));
    }
}
