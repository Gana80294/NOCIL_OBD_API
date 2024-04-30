using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.CommonData
{
    public class NocilRelatedEmployee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public string Employee_Name { get; set; }
        [Required]
        public string Type_Of_Relation { get; set; }

        [ForeignKey("Form_Id")]
        public Form Form { get; set; } = null;
    }
}
