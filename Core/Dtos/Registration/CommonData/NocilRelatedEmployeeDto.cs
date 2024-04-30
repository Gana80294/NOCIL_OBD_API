using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class NocilRelatedEmployeeDto
    {
        public int Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public string Employee_Name { get; set; }
        [Required]
        public string Type_Of_Relation { get; set; }
    }
}
