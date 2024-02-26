using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Facturas.Tests.Mocks
{
    public class AuthorizationServiceMock : IAuthorizationService
    {
        public AuthorizationResult Resultado {  get; set; } 
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            return Task.FromResult(Resultado);
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            return Task.FromResult(Resultado);
        }
    }
}
