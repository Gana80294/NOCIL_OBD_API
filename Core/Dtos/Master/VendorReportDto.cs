﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Master
{
    public class VendorReportDto
    {
        public List<string> Vendor_Category { get; set; } = new List<string>();
        public string? Country_Code { get; set; }
        public int? Region_Id { get; set; }
        public List<int> Type_of_Organization { get; set; } = new List<int>();
        public List<int> Type_of_Vendor { get; set; } = new List<int>();
        public DateTime? ISO_Due_Date { get; set; }
        public bool? Nicerglobe_Registration { get; set; }
    }
}
