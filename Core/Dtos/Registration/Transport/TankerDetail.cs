using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Domain.Core.Entities.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.Transport
{
    public class TankerDetail_Dto
    {
        public int Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public int Tanker_Type_Id { get; set; }
        [Required]
        public float Capacity_of_Tanker { get; set; }

    }
}
