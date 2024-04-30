using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.CommonData
{
    public class ProprietorOrPartner
    {
        [Key]
        public int Id { get; set; }
        public int Form_Id { get; set; }
        public string Name { get; set; }
        public float PercentageShare { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
