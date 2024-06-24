using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class Title
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Title_Name { get; set; }
        public bool Is_Deleted { get; set; } = false;
    }
}
