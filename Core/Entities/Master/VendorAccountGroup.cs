using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class VendorAccountGroup
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }

        public string Description { get; set; }
        public bool Is_Deleted { get; set; } = false;
    }
}
