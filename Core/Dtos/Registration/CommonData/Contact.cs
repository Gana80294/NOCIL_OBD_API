using NOCIL_VP.Domain.Core.Entities.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NOCIL_VP.Domain.Core.Dtos.Registration.CommonData
{
    public class Contact_Dto
    {
        public int Contact_Id { get; set; }
        [Required]
        public int Form_Id { get; set; }
        [Required]
        public int Contact_Type_Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Designation { get; set; }
        [Required]
        public string Email_Id { get; set; }
        [MaxLength(15)]
        public string Phone_Number { get; set; }
        [MaxLength(15)]
        public string Mobile_Number { get; set; }
    }
}
