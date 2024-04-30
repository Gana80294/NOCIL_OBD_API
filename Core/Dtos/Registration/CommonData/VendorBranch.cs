using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class VendorBranch_Dto
    {
        public int Branch_Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Email_Id { get; set; }
        [Required,MaxLength(15)]
        public string Mobile_No { get; set; }
        [Required]
        public string Location { get; set; }
    }
}
