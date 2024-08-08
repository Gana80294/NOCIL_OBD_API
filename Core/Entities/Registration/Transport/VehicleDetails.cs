using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.Transport
{
    public class VehicleDetails
    {
        [Key]
        public int VehicleTypeId { get; set; }
        public int Form_Id { get; set; }
        public string Vehicle_Type { get; set; }
        public float Capacity { get; set; }


        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
