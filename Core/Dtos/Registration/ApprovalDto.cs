using NOCIL_VP.Domain.Core.Dtos.Registration.CommonData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration
{
    public class ApprovalDto
    {
        public int Form_Id { get; set; }
        public int VendorTypeId { get; set; }
        public string EmployeeId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RmEmployeeId { get; set; }
        public int RmRoleId { get; set; }                                              
        public string RmRoleName { get; set; }

        public AdditionalFields_Dto? AdditionalFields { get; set; } = null;
    }
}
