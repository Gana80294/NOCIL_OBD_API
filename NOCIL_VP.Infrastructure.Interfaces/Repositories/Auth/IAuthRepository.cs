using NOCIL_VP.Domain.Core.Dtos.Auth;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<AuthenticationResponse> AuthenticateUser(LoginDetails loginDetails);
        Task<User> UpdatePassword(UpdatePassword updatePassword);
        Task<ResponseMessage> RequestOtpForVendorLogin(RequestOtp requestOtp);
        Task<AuthenticationResponse> VerifyOtpForVendorLogin(VerifyOtp otp);
        Task<ResponseMessage> ForgotPassword(ForgotPassword forgotPassword);
        Task<ResponseMessage> RequestOtpForForgotPassword(string employee_Id);
    }
}
