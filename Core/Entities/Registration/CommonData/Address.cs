using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOCIL_VP.Domain.Core.Entities.Master;

namespace NOCIL_VP.Domain.Core.Entities.Registration.CommonData
{
    public class Address
    {

        [Key]
        public int Address_Id { get; set; }
        public int Form_Id { get; set; }
        public int Address_Type_Id { get; set; }
        public string AddressData { get; set; }
        [MaxLength(20)]
        public string Tel { get; set; }
        [MaxLength(20)]
        public string Fax { get; set; }
        [MaxLength(100)]
        public string Website { get; set; }

        [ForeignKey("Address_Type_Id")]
        public virtual AddressType AddressTypes { get; set; } = null;
        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
