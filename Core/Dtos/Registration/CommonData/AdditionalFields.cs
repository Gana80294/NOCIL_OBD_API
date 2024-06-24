using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class AdditionalFields_Dto
    {
        public int Id { get; set; }

        [Required]
        public int Form_Id { get; set; }

        [Required]
        public int Industry_Id { get; set; }
        [Required]

        public int Incoterms_Id { get; set; }
        [Required]

        public int Reconciliation_Id { get; set; }
        [Required]

        public int Schema_Id { get; set; }

        [Required]
        public string Language { get; set; }

        public string Order_Currency { get; set; }
        [Required]
        public string GrBased { get; set; }
        [Required]
        public string SrvBased { get; set; }

        [Required]
        public string Search_Term { get; set; }
    }
}
