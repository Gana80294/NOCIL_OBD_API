using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class ProprietorOrPartner_Dto
    {
        public int Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float PercentageShare { get; set; }
    }
}
