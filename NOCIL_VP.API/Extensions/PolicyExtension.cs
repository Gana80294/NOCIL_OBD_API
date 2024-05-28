using Microsoft.AspNetCore.Authorization;
using NOCIL_VP.API.Auth;

namespace NOCIL_VP.API.Extensions
{
    public static class PolicyExtension
    {
        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            //Role Authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.RequireRole("Admin");
                });

                options.AddPolicy("VendorPolicy", policy =>
                {
                    policy.RequireRole("Vendor");
                });

                options.AddPolicy("NonVendorPolicy", policy =>
                    policy.Requirements.Add(new RoleMatchRequirement(
                        includeRoles: new[] { "Admin", "PO", "RM", "Manager" },
                        excludeRoles: new string[] { "Vendor" })));

                options.AddPolicy("NonAdminPolicy", policy =>
                    policy.Requirements.Add(new RoleMatchRequirement(
                        includeRoles: new[] { "Vendor", "PO", "RM", "Manager" },
                        excludeRoles: new[] { "Admin" })));

                options.AddPolicy("BuyerPolicy", policy =>
                    policy.Requirements.Add(new RoleMatchRequirement(
                        includeRoles: new[] { "PO", "RM", "Manager" },
                        excludeRoles: new[] { "Admin", "Vendor" })));

                options.AddPolicy("POPolicy", policy =>
                    policy.Requirements.Add(new RoleMatchRequirement(
                        includeRoles: new[] { "PO" },
                        excludeRoles: new[] { "Admin", "Vendor", "RM", "Manager" })));

                options.AddPolicy("AdminBuyerPolicy", policy =>
                    policy.Requirements.Add(new RoleMatchRequirement(
                        includeRoles: new[] { "Admin", "Vendor" },
                        excludeRoles: new[] { "PO", "RM", "Manager" })));

            });

            services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
            return services;
        }
    }
}
