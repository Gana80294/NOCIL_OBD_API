using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NOCIL_VP.Domain.Core.Configurations;
using NOCIL_VP.Domain.Core.Dtos.Auth;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Auth;
using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly VpContext _dbContext;
        private readonly OtpHelper _otpHelper;
        private readonly JwtSetting _jwtSetting;
        private readonly AppSetting _appSetting;

        public AuthRepository(VpContext context, OtpHelper otpHelper, IConfiguration config)
        {
            this._dbContext = context;
            this._otpHelper = otpHelper;
            _jwtSetting = config.GetSection("JWTSecurity").Get<JwtSetting>();
            _appSetting = config.GetSection("AppSettings").Get<AppSetting>();
        }

        // User Authentication method
        public async Task<AuthenticationResponse> AuthenticateUser(LoginDetails loginDetails)
        {
            AuthenticationResponse authResponse = new AuthenticationResponse();
            var user = this._dbContext.Users.FirstOrDefault(u =>
            ((u.Employee_Id == loginDetails.UserName || u.Email == loginDetails.UserName)));

            if (user == null)
            {
                throw new Exception("Username or password is wrong");
            }

            if (PasswordEncryptor.VerifyPassword(loginDetails.Password, user.Password))
            {
                var roles = await (from role in _dbContext.Roles
                                   join userRole in _dbContext.User_Role_Mappings on role.Role_Id equals userRole.Role_Id
                                   where userRole.Employee_Id == user.Employee_Id || userRole.Employee_Id == user.Reporting_Manager_EmpId
                                   select new
                                   {
                                       EId = userRole.Employee_Id,
                                       Role = role.Role_Name,
                                       RoleId = userRole.Role_Id
                                   }).ToListAsync();
                authResponse.Employee_Id = user.Employee_Id;
                authResponse.Role = roles.FirstOrDefault(x => x.EId == user.Employee_Id)?.Role;
                authResponse.Role_Id = roles.FirstOrDefault(x => x.EId == user.Employee_Id) != null ? roles.FirstOrDefault(x => x.EId == user.Employee_Id).RoleId : 0;
                authResponse.RmEmployee_Id = user.Reporting_Manager_EmpId;
                authResponse.RmRole_Id = roles.FirstOrDefault(x => x.EId == user.Reporting_Manager_EmpId) != null ? roles.FirstOrDefault(x => x.EId == user.Reporting_Manager_EmpId).RoleId : 0;
                authResponse.RmRole = roles.FirstOrDefault(x => x.EId == user.Reporting_Manager_EmpId)?.Role;
                authResponse.FirstName = user.First_Name;
                authResponse.MiddleName = user.Middle_Name;
                authResponse.LastName = user.Last_Name;
                authResponse.DisplayName = $"{user.First_Name} {user.Middle_Name} {user.Last_Name}".TrimEnd();
                authResponse.Email = user.Email;
                authResponse.Mobile = user.Mobile_No;
                authResponse.Token = GenerateToken(authResponse);

                return authResponse;
            }
            else
            {
                throw new Exception("Username or Password is incorrect");
            }
        }

        // Access token generation method
        private string GenerateToken(AuthenticationResponse authenticationResponse)
        {
            string securityKey = _jwtSetting.securityKey;
            string issuer = _jwtSetting.issuer;
            string audience = _jwtSetting.audience;

            //symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            //signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //add claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authenticationResponse.DisplayName),
                new Claim(ClaimTypes.Role, authenticationResponse.Role)
            };
            if (!string.IsNullOrWhiteSpace(authenticationResponse.Employee_Id) && authenticationResponse.Role != "Vendor")
            {
                claims.Add(new Claim("EmployeeId", authenticationResponse.Employee_Id));
            } 

            if(!string.IsNullOrWhiteSpace(authenticationResponse.Employee_Id) && authenticationResponse.Role == "Vendor")
            {
                claims.Add(new Claim("VendorCode", authenticationResponse.Employee_Id));
            }

            if (string.IsNullOrWhiteSpace(authenticationResponse.Employee_Id) && authenticationResponse.Role == "Vendor")
            {
                claims.Add(new Claim("VendorCode", "not_registered"));
            }

            var token = new JwtSecurityToken(
                                issuer: issuer,
                                audience: audience,
                                expires: DateTime.Now.AddHours(4),
                                signingCredentials: signingCredentials,
                                claims: claims
                            );
            var authToken = new JwtSecurityTokenHandler().WriteToken(token);
            return authToken;
        }

        // Update password method
        public async Task<User> UpdatePassword(UpdatePassword updatePassword)
        {
            User userResult = new User();

            User user = this._dbContext.Users.FirstOrDefault(u => u.Employee_Id == updatePassword.EmployeeId);
            bool isOldPasswordCorrect = PasswordEncryptor.VerifyPassword(updatePassword.CurrentPassword, user.Password);
            if (isOldPasswordCorrect)
            {
                string defaultPassword = _appSetting.DefaultPassword;

                if (updatePassword.NewPassword == defaultPassword)
                {
                    userResult = null;
                    throw new Exception("New Password can not be same as default password.");
                }
                else
                {
                    user.Password5 = user.Password4;
                    user.Password4 = user.Password3;
                    user.Password3 = user.Password2;
                    user.Password2 = user.Password1;
                    user.Password1 = user.Password;
                    user.Password = PasswordEncryptor.EncryptPassword(updatePassword.NewPassword);
                    this._dbContext.Users.Update(user);
                    await this._dbContext.SaveChangesAsync();
                    userResult = user;
                }
            }
            else
            {
                throw new Exception("Current Password is wrong");
            }
            return userResult;
        }


        // Forgot Password
        public async Task<ResponseMessage> ForgotPassword(ForgotPassword forgotPassword)
        {
            var txn = this._dbContext.ForgotPasswordOtpTransactions.FirstOrDefault(o => o.Employee_Id == forgotPassword.Employee_Id);
            if (txn == null)
            {
                throw new Exception("Unable to find the transaction - Kindly requet OTP again");
            }
            else
            {
                var otp = new VerifyOtp()
                {
                    Otp = forgotPassword.Otp,
                    TxId = txn.TxId,
                };
                var validate = await this._otpHelper.VerifyOtp(otp);
                if (validate)
                {
                    var user = this._dbContext.Users.FirstOrDefault(x => x.Employee_Id == forgotPassword.Employee_Id);
                    if (user == null)
                    {
                        throw new Exception("Unable to find the user");
                    }
                    user.Password5 = user.Password4;
                    user.Password4 = user.Password3;
                    user.Password3 = user.Password2;
                    user.Password2 = user.Password1;
                    user.Password1 = user.Password;
                    user.Password = PasswordEncryptor.EncryptPassword(forgotPassword.Password);

                    await this._dbContext.SaveChangesAsync();
                    return ResponseWritter.WriteSuccessResponse("Password updated successfully");
                }
                else
                {
                    throw new Exception("Unable to verify OTP - server error");
                }
            }

        }

        // Request for OTP
        public async Task<ResponseMessage> RequestOtpForVendorLogin(RequestOtp requestOtp)
        {

            var res = await this._otpHelper.SendOTP(requestOtp.Mobile);
            if (res != null)
            {
                var otpTransaction = this._dbContext.OtpTransactions.FirstOrDefault(x => x.Form_Id == requestOtp.FormId);
                if (otpTransaction != null)
                {
                    otpTransaction.Requested_On = DateTime.Now;
                    otpTransaction.TxId = res.TxId;
                    otpTransaction.Mobile = requestOtp.Mobile;
                    otpTransaction.Validated_On = (DateTime?)null;
                }
                else
                {
                    otpTransaction = new OtpTransaction()
                    {
                        TxId = res.TxId,
                        Mobile = requestOtp.Mobile,
                        Form_Id = requestOtp.FormId,
                        Requested_On = DateTime.Now
                    };
                    this._dbContext.OtpTransactions.Add(otpTransaction);
                }
                await _dbContext.SaveChangesAsync();
                return ResponseWritter.WriteSuccessResponse(res.Message.ToString());
            }
            else
            {
                throw new Exception("Unable to send OTP - Server error");
            }
        }

        public async Task<ResponseMessage> RequestOtpForForgotPassword(string employee_Id)
        {
            var user = this._dbContext.Users.FirstOrDefault(u => u.Employee_Id == employee_Id);
            if (user == null)
            {
                throw new Exception("Invalid Employee ID - User not found");
            }
            var mobile = user.Mobile_No.Contains("+91") ? user.Mobile_No : "+91" + user.Mobile_No.ToString();
            var res = await _otpHelper.SendOTP(mobile);
            if (res != null)
            {
                var otpTransaction = this._dbContext.ForgotPasswordOtpTransactions.FirstOrDefault(x => x.Employee_Id == employee_Id);
                if (otpTransaction != null)
                {
                    otpTransaction.Requested_On = DateTime.Now;
                    otpTransaction.TxId = res.TxId;
                    otpTransaction.Validated_On = (DateTime?)null;
                }
                else
                {
                    otpTransaction = new ForgotPasswordOtpTransaction()
                    {
                        Employee_Id = employee_Id,
                        Requested_On = DateTime.Now,
                        TxId = res.TxId,
                        Validated_On = (DateTime?)null,
                    };
                    this._dbContext.ForgotPasswordOtpTransactions.Add(otpTransaction);
                }
                await this._dbContext.SaveChangesAsync();
                return ResponseWritter.WriteSuccessResponse(res.Message.ToString());
            }
            else
            {
                throw new Exception("Unable to send OTP - Server error");
            }
        }


        // Verify OTP
        public async Task<AuthenticationResponse> VerifyOtpForVendorLogin(VerifyOtp otp)
        {
            var txn = this._dbContext.OtpTransactions.FirstOrDefault(x => x.Form_Id == otp.FormId);
            if (txn == null) { throw new Exception("Unable to find the transaction - Kindly requet OTP again"); }
            otp.TxId = txn.TxId;
            var validate = await this._otpHelper.VerifyOtp(otp);
            if (validate)
            {
                var role = this._dbContext.Roles.FirstOrDefault(x => x.Role_Name == "Vendor");
                var form = this._dbContext.Forms.FirstOrDefault(x => x.Form_Id == otp.FormId);
                AuthenticationResponse authResponse = new AuthenticationResponse();
                authResponse.Role = role.Role_Name;
                authResponse.Role_Id = role.Role_Id;
                authResponse.DisplayName = form.Vendor_Name;
                authResponse.Mobile = otp.Mobile;
                authResponse.Token = GenerateToken(authResponse);

                return authResponse;
            }
            else
            {
                throw new Exception("Unable to verify OTP - server error");
            }
        }

    }
}
