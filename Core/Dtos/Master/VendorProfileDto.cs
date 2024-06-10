using NOCIL_VP.Domain.Core.Dtos.Registration.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Master
{
    public class VendorProfileDto
    {
        public string Vendor_Name { get; set; }
        public string Year { get; set; }
        public VendorGradeDto Grade { get; set; }
    }
}
