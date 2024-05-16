using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class TankerType
    {
        [Key]
        public int Tanker_Type_Id { get; set; }
        [Required, MaxLength(50)]
        public string Tanker_Type { get; set; }
        public bool Is_Deleted { get; set; } = false;
    }
}
