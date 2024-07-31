using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.Domestic
{
    public class VendorPersonalData_Dto
    {
        public int Personal_Info_Id { get; set; }
        [Required(ErrorMessage = "Domestic vendor personal data - Form Id is empty")]
        public int Form_Id { get; set; }
        [Required(ErrorMessage = "Domestic vendor personal data - Organization name is empty")]
        public string Organization_Name { get; set; }
        [Required]
        public int Plant_Installation_Year { get; set; }

        public int Title_Id { get; set; }
        public int? GSTVenClass_Id { get; set; }
    }
}
