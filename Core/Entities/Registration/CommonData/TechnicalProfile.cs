using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.CommonData
{
    public class TechnicalProfile
    {
        [Key]
        public int Id { get; set; }
        public int Form_Id { get; set; }
        public bool Is_ISO_Certified { get; set; }
        public bool Other_Qms_Certified { get; set; }
        public bool Planning_for_Qms { get; set; } = false;
        public bool Is_Statutory_Provisions_Adheard { get; set; }
        public string Initiatives_for_Development { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
