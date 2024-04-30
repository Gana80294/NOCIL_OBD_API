using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration.Reason
{
    public class ReasonDto
    {
        public bool IsRejected { get; set; }
        public List<ReasonDetailDto> Reasons { get; set; }
    }
}
