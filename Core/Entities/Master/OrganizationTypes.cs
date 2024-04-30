using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class OrganizationType
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Type_of_Organization { get; set; }
    }
}
