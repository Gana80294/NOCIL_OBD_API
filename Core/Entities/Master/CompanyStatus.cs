using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class CompanyStatus
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Company_Status { get; set; }
    }
}
