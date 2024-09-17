using NOCIL_VP.Domain.Core.Entities.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Logs
{
    public class EditRequestHistory
    {
        [Key]
        public long Log_Id { get; set; }
        public int Form_Id { get; set; }
        public string VendorCode { get; set; }
        public DateTime RequestedOn { get; set; }
        public bool IsApproved { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string? Rejectedby { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; }
    }
}
