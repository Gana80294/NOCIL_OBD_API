using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Master
{
    public class VendorMasterDto
    {
        public string Vendor_Name { get; set; }
        public string Vendor_Mail { get; set; }
        public string Vendor_Mobile { get; set; }
        public string Vendor_Code { get; set; }
        public string Vendor_Type { get; set; }
        public int Form_Id { get; set; }
        public int VT_Id { get; set; }
        public string? Doc_Type { get; set; }
        public DateTime? Expiry_Date { get; set; }
    }
}
