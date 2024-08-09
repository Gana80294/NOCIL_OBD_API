using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class CommercialProfile_Dto
    {
        public int Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        public string Financial_Credit_Rating { get; set; }
        public string Agency_Name { get; set; }
        [MaxLength(10, ErrorMessage = "PAN number sould be 10 charecter length"),
            RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]$", ErrorMessage = "Invalid PAN number format")]
        public string PAN { get; set; }
        [MaxLength(15, ErrorMessage = "GSTIN number sould be 10 charecter length")]
        [RegularExpression(@"^([0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}[0-9A-Z]{2})+$", ErrorMessage = "Invalid GST number format")]
        public string GSTIN { get; set; }
        public bool Is_MSME_Type { get; set; }
        public string MSME_Type { get; set; }
        public string MSME_Number { get; set; }
        public string ServiceCategory { get; set; }
    }
}
