using NOCIL_VP.Domain.Core.Entities.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Auth
{
    public class OtpTransaction
    {
        [Key]
        public int Id { get; set; }
        public int Form_Id { get; set; }
        public string TxId { get; set; }
        public string Mobile { get; set; }
        public DateTime Requested_On { get; set; }
        public DateTime? Validated_On { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
