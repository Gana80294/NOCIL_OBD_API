using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Auth
{

    public class RequestOtp
    {
        public int FormId { get; set; }
        public string Mobile { get; set; }
    }

    public class VerifyOtp
    {
        public string Otp { get; set; }
        public string Mobile { get; set; }
        public int FormId { get; set; }
        public string TxId { get; set; }
    }

}
