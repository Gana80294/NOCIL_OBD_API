using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public  class GSTVenClass
    {

        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }
        public bool Is_Deleted { get; set; } = false;
    }
}
