using NOCIL_VP.Domain.Core.Entities.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Registration.Attachments
{
    public class Attachment
    {
        [Key]
        public int Attachment_Id { get; set; }
        public int Form_Id { get; set; }
        public string File_Name { get; set; }
        public string File_Path { get; set; }
        public string File_Extension { get; set; }
        public string File_Type { get; set; }
        public bool Is_Expiry_Available { get; set; }
        public DateTime? Expiry_Date { get; set; }


        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; } = null;
    }
}
