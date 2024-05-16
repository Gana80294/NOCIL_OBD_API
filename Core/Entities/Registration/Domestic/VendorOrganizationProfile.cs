using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.Domestic
{
    public class VendorOrganizationProfile
    {
        [Key]
        public int Id { get; set; }
        public int Form_Id { get; set; }
        public int Type_of_Org_Id { get; set; }
        public int Status_of_Company_Id { get; set; }
        public float Annual_Prod_Capacity { get; set; } = 0;
        public bool RelationToNocil { get; set; }
        public string Unit { get; set; }


        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
        [ForeignKey("Type_of_Org_Id")]
        public virtual OrganizationType OrganizationTypes { get; set; } = null;
        [ForeignKey("Status_of_Company_Id")]
        public virtual CompanyStatus CompanyStatus { get; set; } = null;
    }
}
