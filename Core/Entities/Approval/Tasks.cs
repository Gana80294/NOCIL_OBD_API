using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Domain.Core.Entities.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Approval
{
    public class Tasks
    {
        [Key]
        public long Task_Id { get; set; }
        public int Form_Id { get; set;}
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Owner_Id { get; set; }
        public int? Role_Id { get; set; }
        public int Level { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
        [ForeignKey("Owner_Id")]
        public virtual User User { get; set; } = null;
        [ForeignKey("Role_Id")]
        public virtual Role Role { get; set; } = null;
    }
}
