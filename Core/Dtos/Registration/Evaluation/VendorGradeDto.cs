using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.Evaluation
{
    public class VendorGradeDto
    {
        public int GradeId { get; set; }
        public int FormId { get; set; }
        public string Vendor_Code { get; set; }
        public float Grade { get; set; }
        public DateTime Last_Audit_Date { get; set; }
        public string Last_Audited_By { get; set; }
        public string Location { get; set; }
    }
}
