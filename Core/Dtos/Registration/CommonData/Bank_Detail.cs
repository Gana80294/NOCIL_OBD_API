using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class Bank_Detail_Dto
    {
        public int Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required,MaxLength(100)]
        public string AccountHolder { get; set; }
        [Required,MaxLength(100)]
        public string Bank { get; set; }
        [Required,MaxLength(30)]
        public string Branch { get; set; }
        public string Address { get; set; }
        [MaxLength(30)]
        public string Account_Number { get; set; }
        [MaxLength(11,ErrorMessage = "IFSC Code should be in 11 charecters length")]
        public string IFSC { get; set; }
        [MaxLength(11, ErrorMessage = "SWIFT Code length can not exceed 11 charecters")]
        public string SWIFT { get; set; }
        [MaxLength(34, ErrorMessage = "SWIFT Code length can not exceed 34 charecters")]
        public string IBAN { get; set; }
    }
}
