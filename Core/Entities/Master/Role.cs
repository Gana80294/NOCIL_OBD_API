using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class Role
    {
        [Key]
        public int Role_Id { get; set; }
        [Required, MaxLength(50)]
        public string Role_Name { get; set; }
        public bool Is_Deleted { get; set; } = false;
    }
}
