using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Response
{
    public class AuthenticationResponse
    {
        public string Employee_Id { get; set; }
        public string Role { get; set; }
        public int Role_Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Token { get; set; }
        public string RmEmployee_Id { get; set; }
        public int RmRole_Id {  get; set; }
        public string RmRole { get; set; }
    }
}
