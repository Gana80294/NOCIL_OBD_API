using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class FormStatus
    {
        [Key]
        public int Status_Id { get; set; }
        [Required, MaxLength(50)]
        public string Status { get; set; }
    }
}
