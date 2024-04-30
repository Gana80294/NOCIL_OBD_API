using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class AnnualTurnOver_Dto
    {
        public int TurnOver_Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public int Year { get; set; }
        public long SalesTurnOver { get; set; }
        public long OperatingProfit { get; set; }
        public long NetProfit { get; set; }
    }
}
