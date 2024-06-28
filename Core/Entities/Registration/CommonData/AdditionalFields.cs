using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.CommonData
{
    public class AdditionalFields
    {
        [Key]
        public int Id { get; set; }

        public int Form_Id { get; set; }

        public int Industry_Id { get; set; }

        public int? Incoterms_Id { get; set; }
        public int Reconciliation_Id { get; set; }

        public int? Schema_Id { get; set; }

        public int AccountGroup_Id { get; set; }

        [Required]
        public string Language { get; set; }

        public string? Order_Currency { get; set; }
        [Required]
        public string GrBased { get; set; }
        [Required]
        public string SrvBased { get; set; }

        [Required]
        public string Search_Term { get; set; }

        public string? PO_Code { get; set; }


        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;

        [ForeignKey("Industry_Id")]
        public virtual Industry Industry { get; set; } = null;

        [ForeignKey("Incoterms_Id")]
        public virtual Incoterms Incoterms { get; set; } = null;

        [ForeignKey("Reconciliation_Id")]
        public virtual ReconciliationAccount ReconciliationAccount { get; set; } = null;

        [ForeignKey("Schema_Id")]
        public virtual SchemaGroup SchemaGroup { get; set; } = null;

        [ForeignKey("PO_Code")]
        public virtual PurchaseOrganization PurchaseOrganization { get; set; } = null;

        [ForeignKey("AccountGroup_Id")]
        public virtual VendorAccountGroup VendorAccountGroup { get; set; }
    }
}
