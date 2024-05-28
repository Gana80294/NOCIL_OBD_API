using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Dtos.Auth;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Auth;

namespace NOCIL_VP.API.Controllers.Auth
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            this._authRepository = authRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser(LoginDetails loginDetails)
        {
            return Ok(await _authRepository.AuthenticateUser(loginDetails));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(UpdatePassword updatePassword)
        {
            return Ok(await _authRepository.UpdatePassword(updatePassword));
        }

        [HttpGet]
        public async Task<IActionResult> RequestOtpForForgotPassword(string employee_Id)
        {
            return Ok(await this._authRepository.RequestOtpForForgotPassword(employee_Id));
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            return Ok(await this._authRepository.ForgotPassword(forgotPassword));
        }


        [HttpPost]
        public async Task<IActionResult> RequestOtpForVendorLogin(RequestOtp requestOtp)
        {
            return Ok(await _authRepository.RequestOtpForVendorLogin(requestOtp));
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtpForVendorLogin(VerifyOtp otp)
        {
            return Ok(await this._authRepository.VerifyOtpForVendorLogin(otp));
        }
    }
}
