using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Dashboard
{
    public class ExpiryNotificationDto
    {
        public string Vendor_Name { get; set; }
        public string Vendor_Mobile { get; set; }
        public string Vendor_Mail { get; set; }
        public string Doc_Type { get; set; }
        public DateTime Valid_Till_Date { get; set; }
    }
}
