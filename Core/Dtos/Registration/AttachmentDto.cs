using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration
{
    public class AttachmentDto
    {
        public int Attachment_Id { get; set; }
        [Required(ErrorMessage = "Form Id Required")]
        public int Form_Id { get; set; }
        [Required(ErrorMessage = "File_Name Required")]
        public string File_Name { get; set; }
        public string File_Path { get; set; }
        [Required(ErrorMessage = "File_Extension Required")]
        public string File_Extension { get; set; }
        [Required(ErrorMessage = "File_Type Required")]
        public string File_Type { get; set; }
        [Required(ErrorMessage = "Is_Expiry_Available Required")]
        public bool Is_Expiry_Available { get; set; }
        public DateTime? Expiry_Date { get; set; }
    }
}
