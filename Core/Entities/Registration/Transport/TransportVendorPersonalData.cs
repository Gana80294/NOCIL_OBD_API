using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOCIL_VP.Domain.Core.Entities.Registration;
using NOCIL_VP.Domain.Core.Entities.Master;

namespace NOCIL_VP.Domain.Core.Entities.Registration.Transport
{
    public class TransportVendorPersonalData
    {
        [Key]
        public int Id { get; set; }
        public int Form_Id { get; set; }
        public int Title_Id { get; set; }

        public string Name_of_Transporter { get; set; }
        public DateTime Date_of_Establishment { get; set; }
        public int GSTVenClass_Id { get; set; }

        public int No_of_Own_Vehicles { get; set; }
        public int No_of_Drivers { get; set; }
        public bool Nicerglobe_Registration_Status { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;

        [ForeignKey("Title_Id")]
        public virtual Title Title { get; set; } = null;

        [ForeignKey("GSTVenClass_Id")]
        public virtual GSTVenClass GSTVenClass { get; set; } = null;
    }
}
