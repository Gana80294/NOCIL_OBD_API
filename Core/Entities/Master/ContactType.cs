using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class ContactType
    {
        [Key]
        public int Contact_Type_Id { get; set; }
        [Required, MaxLength(50)]
        public string Contact_Type { get; set; }
    }
}
