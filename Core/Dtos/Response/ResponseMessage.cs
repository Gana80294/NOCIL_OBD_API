using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Response
{
    public class ResponseMessage
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
