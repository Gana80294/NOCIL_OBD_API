using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOCIL_VP.Domain.Core.Entities.Registration;

namespace NOCIL_VP.Domain.Core.Entities.Registration.Transport
{
    public class TransportVendorPersonalData
    {
        [Key]
        public int Id { get; set; }
        public int Form_Id { get; set; }
        public string Name_of_Transporter { get; set; }
        public DateTime Date_of_Establishment { get; set; }
        public int No_of_Own_Vehicles { get; set; }
        public int No_of_Drivers { get; set; }
        public bool Nicerglobe_Registration_Status { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
