using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FreelancerWeb.Services
{
    public class JwtTokenService
    {
        private readonly string Token;

        public JwtTokenService(string token)
        {
            Token = token;
        }

        public string GetValueByKey(string key)
        {
            var token_claims = new JwtSecurityTokenHandler().ReadJwtToken(Token);
            if (token_claims != null)
            {
                IEnumerable<Claim> claims = token_claims.Claims;
                if (claims != null && !claims.IsEmpty())
                {
                    var claim = token_claims.Claims.First(c => c.Type == key);
                    if (claim != null)
                    {
                        var claim_value = claim.Value;
                        return claim_value;
                    }
                }
            }
            
            return string.Empty;
        }
    }
}
