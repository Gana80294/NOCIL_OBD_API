using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class Subsideries_Dto
    {
        public int Id { get; set; }
        [MaxLength(200, ErrorMessage = "Company Name can not exceed 200 charecters length")]
        public string Subsidery_Name { get; set; }
        [Required]
        public int Form_Id { get; set; }
    }
}
