using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Approval
{
    public class WorkFlow
    {
        [Key]
        public int WorkFlow_Id { get; set; }
        public int Role_Id { get; set; }
        public int Level { get; set; }
        public int Vendor_Type_Id { get; set; }
        public int GroupId { get; set; }

        [ForeignKey("Role_Id")]
        public virtual Role Roles { get; set; } = null;
        [ForeignKey("Vendor_Type_Id")]
        public virtual VendorType VendorTypes { get; set; } = null;
    }
}
