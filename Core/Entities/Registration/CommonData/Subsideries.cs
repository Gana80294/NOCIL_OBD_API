using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.CommonData
{
    public class Subsideries
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200, ErrorMessage = "Company Name can not exceed 200 charecters length")]
        public string Subsidery_Name { get; set; }
        [Required]
        public int Form_Id { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
