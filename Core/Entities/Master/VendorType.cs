using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class VendorType
    {
        [Key]
        public int Id { get; set; }
        public string Vendor_Type { get; set; }
        public bool Is_Deleted { get; set; } = false;
    }
}
