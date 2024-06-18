using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Dashboard
{
    public class DashboardDto
    {
        public int FormId { get; set; }
        public int VendorTypeId { get; set; }
        public string VendorType { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }
        public string PendingWith { get; set; }
    }

    public class InitialData
    {
        public List<DashboardDto> Data { get; set; }
        public int Open { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int SAP { get; set; }
    }
}
