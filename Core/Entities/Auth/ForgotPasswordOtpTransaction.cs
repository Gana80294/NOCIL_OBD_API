using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Auth
{
    public class ForgotPasswordOtpTransaction
    {
        [Key]
        public int Id { get; set; }
        public string Employee_Id { get; set; }
        public string TxId { get; set; }
        public DateTime Requested_On { get; set; }
        public DateTime? Validated_On { get; set; }

        [ForeignKey("Employee_Id")]
        public virtual User User { get; set; } = null;
    }
}
