using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Master
{
    public class UserDto
    {
        [Required(ErrorMessage = "Employee Id can not be empty")]
        public string Employee_Id { get; set; }
        [Required(ErrorMessage = "Role Id can not be empty")]
        public int Role_Id { get; set; }
        [Required(ErrorMessage = "First name can not be empty")]
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "Email can not be empty")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile number can not be empty")]
        public string Mobile_No { get; set; }
        public string Reporting_Manager_EmpId { get; set; }
        public bool IsActive { get; set; }
        public string Display_Name { get; set; }
    }
}
