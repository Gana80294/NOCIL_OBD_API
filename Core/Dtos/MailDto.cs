using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos
{
    public class SendMail
    {
        public string Username { get; set; }
        public string ToEmail { get; set; }
        public int Form_Id { get; set; }
        public int Vendor_Type_Id { get; set; }
    }

    public class ApprovalMailInfo
    {
        public string Email { get; set; }
        public string Mobile_Number { get; set; }
        public string UserName { get; set; }
        public string ToEmail { get; set; }
        public int Form_Id { get; set; }
    }

    public class ApprovalInfo
    {
        public string UserName { get; set; }
        public string ToEmail { get; set; }
        public string ApprovedBy { get; set; }
    }

    public class RejectionMailInfo
    {
        public int Form_Id { get; set; }
        public string Username { get; set; }
        public int Vendor_Type_Id { get; set; }
        public string ToEmail { get; set; }
        public string Reason { get; set; }

    }

    public class RejectionMailInfoToBuyer
    {
        public int Form_Id { get; set; }
        public string Recepient { get; set; }
        public string ToEmail { get; set; }
        public string Reason { get; set; }
        public string RejectedBy { get; set; }
        public string VendorMail { get; set; }
        public string VendorMobile { get; set; }
        public string VendorName { get; set; }

    }
}
