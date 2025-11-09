using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Iottu.Api.IntegrationTests
{
    public class FakeAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly FakeAuthContext _authContext;

        public FakeAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            FakeAuthContext authContext)
            : base(options, logger, encoder)
        {
            _authContext = authContext;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _authContext.Username),
                new Claim(ClaimTypes.NameIdentifier, _authContext.UserId.ToString())
            };

            foreach (var role in _authContext.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class FakeAuthContext
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = "testuser";
        public List<string> Roles { get; set; } = new() { "admin" };
    }
}
