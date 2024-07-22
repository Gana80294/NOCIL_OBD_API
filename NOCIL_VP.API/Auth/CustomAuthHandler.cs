using Microsoft.IdentityModel.Tokens;
using NOCIL_VP.Domain.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NOCIL_VP.API.Auth
{
    public class CustomAuthHandler
    {
        private readonly VpContext _dbContext;
        public CustomAuthHandler(VpContext context)
        {
            _dbContext = context;
        }

        public bool IsValidToken(string jwtToken, string issuer, string audience, SymmetricSecurityKey signingKey)
        {
            return ValidateToken(jwtToken, issuer, audience, signingKey);
        }

        private bool ValidateToken(string jwtToken, string issuer, string audience, SymmetricSecurityKey signingKey)
        {
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience
                };
                ISecurityTokenValidator tokenValidator = new JwtSecurityTokenHandler();
                var claim = tokenValidator.ValidateToken(jwtToken, validationParameters, out var _);
                var access = ValidatePermission(jwtToken);
                return true;
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException();
            }
        }

        private bool ValidatePermission(string jwtToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtTokenObj = tokenHandler.ReadJwtToken(jwtToken);


                var role = jwtTokenObj.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
                if (role != "Vendor")
                {
                    string employeeId = jwtTokenObj.Claims.FirstOrDefault(c => c.Type == "EmployeeId")!.Value;
                    if (!string.IsNullOrWhiteSpace(employeeId) && !string.IsNullOrWhiteSpace(role))
                    {
                        var user = this._dbContext.Users.FirstOrDefault(x => x.Employee_Id == employeeId);
                        if (user != null) { return true; }
                    }
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    string vendorCode = jwtTokenObj.Claims.FirstOrDefault(c => c.Type == "VendorCode")!.Value;
                    if (!string.IsNullOrWhiteSpace(vendorCode) && !string.IsNullOrWhiteSpace(role))
                    {
                        if (vendorCode == "not_registered")
                        {
                            return true;
                        }
                        var user = this._dbContext.Users.FirstOrDefault(x => x.Employee_Id == vendorCode);
                        if (user != null) { return true; }
                    }
                    throw new UnauthorizedAccessException();
                }
                return true;
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
