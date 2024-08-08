using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Domain.Core.Entities.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.Transport
{
    public class TankerDetail
    {
        [Key]
        public int Id { get; set; }
        public int Form_Id { get; set; }
        public int Tanker_Type_Id { get; set; }
        public float Capacity_of_Tanker { get; set; }
        [MaxLength(50)]
        public string Unit { get; set; }
        public bool IsGPSRegistered { get; set; }


        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
        [ForeignKey("Tanker_Type_Id")]
        public virtual TankerType TankerTypes { get; set; } = null;

    }
}
