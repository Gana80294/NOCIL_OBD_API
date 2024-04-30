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
    public class FormProcess
    {
        [Key]
        public int Process_Id { get; set; }
        public int Form_Id { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_On { get; set; } = DateTime.Now;
        public DateTime? Completed_On { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
        [ForeignKey("Created_By")]
        public virtual User Users { get; set; } = null;
    }
}
