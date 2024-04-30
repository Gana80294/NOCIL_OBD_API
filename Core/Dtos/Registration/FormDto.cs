using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration
{
    public class FormDto
    {
        [Required(ErrorMessage = "Vendor Type is required.")]
        public int Vendor_Type_Id { get; set; }
        public int Status_Id { get; set; } = 0;
        public string Vendor_Code { get; set; }
        [Required(ErrorMessage = "Vendor Name is required.")]
        public string Vendor_Name { get; set; }
        [Required(ErrorMessage = "Vendor Mail is required")]
        public string Vendor_Mail { get; set; }
        [Required(ErrorMessage = "Vendor Mobile Number is required.")]
        public string Vendor_Mobile { get; set; }
        public string Company_Code { get; set; }
        [Required(ErrorMessage = "Department Id is required.")]
        public int Department_Id { get; set; }
        [Required(ErrorMessage = "Created by is required.")]
        public string CreatedBy { get; set; }
    }
}
