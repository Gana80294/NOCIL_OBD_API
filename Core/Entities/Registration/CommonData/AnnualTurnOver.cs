using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.CommonData
{
    public class AnnualTurnOver
    {
        [Key]
        public int TurnOver_Id { get; set; }
        public int Form_Id { get; set; }
        public int Year { get; set; }
        public long SalesTurnOver { get; set; }
        public long OperatingProfit { get; set; }
        public long NetProfit { get; set; }

        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
