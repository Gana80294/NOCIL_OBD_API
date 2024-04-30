using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.Reason
{
    public class ReasonDetailDto
    {
        public string RejectedBy { get; set; }
        public DateTime? RejectedOn { get; set; }
        public string Reason { get; set; }
    }
}
