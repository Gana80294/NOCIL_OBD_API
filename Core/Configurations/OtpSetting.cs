using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Configurations
{
    public class OtpSetting
    {
        public string OtpRequestApi { get; set; }
        public string OtpValidateApi { get; set; }
        public string CustomerKey { get; set; }
        public string ApiKey { get; set; }
        public string TokenKey { get; set; }
        public string Cookie { get; set; }
    }
}
