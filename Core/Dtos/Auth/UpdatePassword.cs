using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Auth
{
    public class UpdatePassword
    {
        public string EmployeeId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ForgotPassword
    {
        public string Employee_Id { get; set; }
        [Required, MinLength(6, ErrorMessage = "OTP should have 6 numbers")]
        public string Otp { get; set; }
        [Required, MinLength(8, ErrorMessage = "Password should have atleast 8 charecters length")]
        public string Password { get; set; }
    }
}
