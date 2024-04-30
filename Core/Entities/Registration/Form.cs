using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration
{
    public class Form
    {
        [Key]
        public int Form_Id { get; set; }
        public int Vendor_Type_Id { get; set; }
        public int Status_Id { get; set; }
        public string Vendor_Code { get; set; }
        public string Vendor_Name { get; set; }
        public string Vendor_Mail { get; set; }
        public string Vendor_Mobile { get; set; }
        public string Company_Code { get; set; }
        public int Department_Id { get; set; }
        public DateTime Created_On { get; set; }
        public string Created_By {  get; set; }

        [ForeignKey("Vendor_Type_Id")]
        public virtual VendorType VendorType { get; set; } = null;
        [ForeignKey("Status_Id")]
        public virtual FormStatus FormStatus { get; set; } = null;
        [ForeignKey("Company_Code")]
        public virtual CompanyCode CompanyCode { get; set; } = null;
        [ForeignKey("Department_Id")]
        public virtual Department DepartmentVirtual { get; set; } = null;
    }
}
