using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.Domestic
{
    public class VendorOrganizationProfile_Dto
    {
        public int Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public int Type_of_Org_Id { get; set; }
        [Required]
        public int Status_of_Company_Id { get; set; }
        [Required]
        public float Annual_Prod_Capacity { get; set; }
        [MaxLength(50)]
        public string Unit { get; set; }
        public bool RelationToNocil { get; set; }
    }
}
