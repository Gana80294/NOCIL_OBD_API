using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Response
{
    public class AttachmentResponse
    {
        public string FileName { get; set; }
        public string DocType { get; set; }
        public byte[] FileContent { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }
    }
}
