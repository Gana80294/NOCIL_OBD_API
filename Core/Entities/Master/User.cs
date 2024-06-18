using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Employee_Id { get; set; }
        [Required, MaxLength(50)]
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Mobile_No { get; set; }
        [Required]
        public string Password { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string Password3 { get; set; }
        public string Password4 { get; set; }
        public string Password5 { get; set; }
        public bool Is_Active { get; set; }
        public string Reporting_Manager_EmpId { get; set; }

    }
}
