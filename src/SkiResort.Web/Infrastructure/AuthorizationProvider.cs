using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdventureWorks.SkiResort.Infrastructure.Model;
using AspNet.Security.OpenIdConnect.Server;
using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;

namespace AdventureWorks.SkiResort.Web.Infrastructure
{
    public sealed class AuthorizationProvider : OpenIdConnectServerProvider
    {
        private readonly IOptions<SecurityConfig> _securityConfig;

        public AuthorizationProvider(IOptions<SecurityConfig> securityConfig)
        {
            _securityConfig = securityConfig;
        }

        public override Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
        {
            if (context.ClientId.Equals(_securityConfig.Value.ClientId)
                &&
                context.ClientSecret.Equals(_securityConfig.Value.ClientSecret))
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            // Only allow resource owner credential flow
            if (!context.Request.IsPasswordGrantType())
            {
                context.Rejected(
                    error: "unsupported_grant_type",
                    description: "Only resource owner credentials " +
                                 "are accepted by this authorization server");
            }

            context.Validated();

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(GrantResourceOwnerCredentialsContext context)
        {
            // Don't inject the UserManager to avoid save a reference for the application lifetime
            // Internally manages an EF DbContext
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

            bool isValidUser = false;
            var user = await userManager.FindByNameAsync(context.UserName);
            if (user != null)
                isValidUser = await userManager.CheckPasswordAsync(user, context.Password);

            if (isValidUser)
            { 
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FullName),
                };

                claims.AddRange(context.Scope.Select(scope => new Claim("scope", scope)));

                var claimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, OpenIdConnectServerDefaults.AuthenticationScheme));

                var ticket = new AuthenticationTicket(
                    claimsPrincipal,
                    new AuthenticationProperties(),
                    OpenIdConnectServerDefaults.AuthenticationScheme);

                context.Validated(ticket);
            }
            else
            {
                context.Rejected();
            }
        }

        public override Task SerializeAccessToken(SerializeAccessTokenContext context)
        {
            context.Audiences.Add(_securityConfig.Value.Audience);

            return Task.FromResult<object>(null);
        }
    }
}
