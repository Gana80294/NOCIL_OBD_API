using NOCIL_VP.Domain.Core.Entities.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NOCIL_VP.Domain.Core.Entities.Registration.CommonData
{
    public class Contact
    {
        [Key]
        public int Contact_Id { get; set; }
        public int Form_Id { get; set; }
        public int Contact_Type_Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Email_Id { get; set; }
        [MaxLength(15)]
        public string Phone_Number { get; set; }
        [MaxLength(15)]
        public string Mobile_Number { get; set; }

        [ForeignKey("Contact_Type_Id")]
        public virtual ContactType ContactTypes { get; set; }
        [ForeignKey("Form_Id")]
        public virtual Form Forms { get; set; }
    }
}
