using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Configurations
{
    public class AppSetting
    {
        public string DefaultPassword { get; set; }
        public string AttachmentFolderPath { get; set; }
        public string SapVendorCreate { get; set; }
        public string SoapVersion { get; set; }
    }
}
