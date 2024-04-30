using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOCIL_VP.Domain.Core.Entities.Master;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class Address_Dto
    {
        public int Address_Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public int Address_Type_Id { get; set; }
        [Required]
        public string AddressData { get; set; }
        [MaxLength(20)]
        public string Tel { get; set; }
        [MaxLength(20)]
        public string Fax { get; set; }
        [MaxLength(100)]
        public string Website { get; set; }
    }
}
