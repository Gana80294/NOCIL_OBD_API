using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.Transport
{
    public class VehicleDetails_Dto
    {
        public int VehicleTypeId { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public string Vehicle_Type { get; set; }
        [Required]
        public float Capacity { get; set; }
    }
}
