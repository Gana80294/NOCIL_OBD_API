﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NOCIL_VP.Domain.Core.Configurations;
using NOCIL_VP.Domain.Core.Entities;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.API.Auth
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSetting _jwtSetting;

        public AuthMiddleware(RequestDelegate next, IOptions<JwtSetting> setting)
        {
            _next = next;
            _jwtSetting = setting.Value;
        }

        public async Task Invoke(HttpContext httpContext, VpContext context)
        {
            var path = httpContext.Request.Path;
            var endPoint = path.Value!.ToString().ToLower();
            if (endPoint.Contains("authenticateuser")
                || endPoint.Contains("requestotpforforgotpassword")
                || endPoint.Contains("forgotpassword")
                || endPoint.Contains("requestotpforvendorlogin")
                || endPoint.Contains("verifyotpforvendorlogin")
                || endPoint.Contains("getsingleformdata"))
            {
                await _next(httpContext);
            }
            else
            {
                try
                {
                    string token = string.Empty;
                    string issuer = _jwtSetting.issuer; //Get issuer value from your configuration
                    string audience = _jwtSetting.audience; //Get audience value from your configuration
                    SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.securityKey));

                    CustomAuthHandler authHandler = new CustomAuthHandler(context);
                    var header = httpContext.Request.Headers["Authorization"];
                    if (header.Count == 0) throw new Exception("Authorization header is empty");
                    string[] tokenValue = Convert.ToString(header).Trim().Split(" ");
                    if (tokenValue.Length > 1) token = tokenValue[1];
                    else throw new Exception("Authorization token is empty");
                    if (authHandler.IsValidToken(token, issuer, audience, signingKey))
                        await _next(httpContext);

                }
                catch (Exception)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    HttpResponseWritingExtensions.WriteAsync(httpContext.Response, "{\"message\": \"Unauthorized\"}").Wait();
                }
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
