using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class TaxBase
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(10)]
        public string Percent { get; set; }
        public bool Is_Deleted { get; set; } = false;
    }
}
