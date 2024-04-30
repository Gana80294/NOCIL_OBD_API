using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration
{
    public class FormSubmitTemplate
    {
        private int FormId;
        private int VendorTypeId;
        public int Form_Id
        {
            get { return FormId; }
            set
            {
                if (value != 0) this.FormId = value;
                else throw new Exception("Form Id can't be zero");
            }
        }
        public int Vendor_Type_Id 
        {
            get { return VendorTypeId; }
            set
            {
                if (value != 0) this.VendorTypeId = value;
                else throw new Exception("Vendor Type Id can't be zero");
            }
        }
        public object FormData { get; set; }
    }
}
