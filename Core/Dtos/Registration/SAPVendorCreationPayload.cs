using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration
{
    public class SAPVendorCreationPayload
    {
        public string Company_code { get; set; }
        public string Purchasing_org { get; set; }
        public string Account_grp { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Search_term { get; set; }
        public string Street_House_number { get; set; }
        public string Street_2 { get; set; }
        public string Street_3 { get; set; }
        public string Street_4 { get; set; }
        public string District { get; set; }
        public string Postal_Code { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Language { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mobile_Phone { get; set; }
        public string E_Mail { get; set; }
        public string Tax_Number_3 { get; set; }
        public string Industry { get; set; }
        public string Initiators_Name { get; set; }
        public string Pan_Number { get; set; }
        public string GST_Ven_Class { get; set; }
        public string First_name { get; set; }
        public string Recon_account { get; set; }
        public string Order_currency { get; set; }
        public string Incoterms { get; set; }
        public string Incoterms_Text { get; set; }
        public string Schema_Group_Vendor { get; set; }
        public string GR_based_Inv_Verif { get; set; }
        public string SRV_based_Inv_Verif { get; set; }
    }
}
