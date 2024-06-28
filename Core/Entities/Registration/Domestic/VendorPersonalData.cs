using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Domain.Core.Entities.Registration;

namespace NOCIL_VP.Domain.Core.Entities.Registration.Domestic
{
    public class VendorPersonalData
    {
        [Key]
        public int Personal_Info_Id { get; set; }
        public int Form_Id { get; set; }
        public string Organization_Name { get; set; }
        public int Plant_Installation_Year { get; set; }

        public int Title_Id { get; set; }
        public int? GSTVenClass_Id { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;

        [ForeignKey("Title_Id")]
        public virtual Title Title { get; set; } = null;

        [ForeignKey("GSTVenClass_Id")]
        public virtual GSTVenClass GSTVenClass { get; set; } = null;
    }
}
