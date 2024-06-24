using NOCIL_VP.Domain.Core.Entities.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class Region
    {
        [Key]
        public int Id { get; set; }
        public string Country_Code { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }


        [MaxLength(100)]
        public string Name { get; set; }
        public bool Is_Deleted { get; set; } = false;

        [ForeignKey("Country_Code")]
        public virtual Country Country { get; set; } = null;

    }
}
