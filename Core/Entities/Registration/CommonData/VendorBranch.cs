using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.CommonData
{
    public class VendorBranch
    {
        [Key]
        public int Branch_Id { get; set; }
        public int Form_Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Email_Id { get; set; }
        [MaxLength(15)]
        public string Mobile_No { get; set; }
        public string Location { get; set; }


        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
