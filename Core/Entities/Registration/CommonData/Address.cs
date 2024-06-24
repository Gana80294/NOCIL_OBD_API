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
       
        [Required,MaxLength(60)]
        public string House_No {  get; set; }

        [Required,MaxLength(40)]
        public string Street_2 {  get; set; }

        [Required, MaxLength(40)]
        public string Street_3 { get; set; }

        [Required, MaxLength(40)]
        public string Street_4 { get; set; }
        [MaxLength(8)]
        public string District {  get; set; }
        [MaxLength(10)]
        public string Postal_Code { get; set; }
        [MaxLength(40)]
        public string City {  get; set; }
       
        public string Country_Code {  get; set; }

        public int Region_Id {  get; set; }

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

        [ForeignKey("Country_Code")]
        public virtual Country Country { get; set; } = null;

        [ForeignKey("Region_Id")]
        public virtual Region Region { get; set; } = null;
    }
}
