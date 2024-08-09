using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOCIL_VP.Domain.Core.Entities.Registration;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.Transport
{
    public class TransportVendorPersonalData_Dto
    {
        public int Id { get; set; }
        [Required]
        public int Form_Id { get; set; }

        public int Title_Id { get; set; }

        [Required]
        public string Name_of_Transporter { get; set; }
        public int Year_of_Establishment { get; set; }

        public int? GSTVenClass_Id { get; set; }

        [Required]
        public int No_of_Own_Vehicles { get; set; }
        [Required]
        public int No_of_Drivers { get; set; }
        public bool Nicerglobe_Registration { get; set; }
    }
}
