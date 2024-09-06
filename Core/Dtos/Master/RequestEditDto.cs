using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Master
{
    public class RequestEditDto
    {
        public int Form_Id { get; set; }
        public string Employee_Id { get; set; }
        public string Reason { get; set; }
    }
}
