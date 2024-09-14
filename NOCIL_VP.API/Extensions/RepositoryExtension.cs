using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NOCIL_VP.API.Auth;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Data.Repositories.Auth;
using NOCIL_VP.Infrastructure.Data.Repositories.Dashboard;
using NOCIL_VP.Infrastructure.Data.Repositories.Master;
using NOCIL_VP.Infrastructure.Data.Repositories.Registration;
using NOCIL_VP.Infrastructure.Data.Repositories.Registration.Evaluation;
using NOCIL_VP.Infrastructure.Data.Repositories.Registration.UpdateForm;
using NOCIL_VP.Infrastructure.Data.Repositories.Reports;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Auth;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Dashboard;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration.Evaluation;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration.UpdateForm;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Reports;
using System.Text;

namespace NOCIL_VP.API.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMasterRepository, MasterRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IDomesticAndImportRepository, DomesticAndImportRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<ITransportRepository, TransportRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IUpdateFormRepository, UpdateFormRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<INotificationsRepository, NotificationsRepository>();
            services.AddScoped<IVendorGradeRepository, VendorGradeRepository>();
            services.AddScoped<IEditRequestRepository, EditRequestRepository>();


            services.AddScoped<OtpHelper>();
            services.AddScoped<GSTHelper>();
            services.AddScoped<EmailHelper>();
            services.AddScoped<ExcelHelper>();
            services.AddScoped<VendorService>();


            return services;
        }
    }
}
