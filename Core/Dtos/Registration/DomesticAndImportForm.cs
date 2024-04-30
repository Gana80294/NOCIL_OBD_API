using NOCIL_VP.Domain.Core.Dtos.Registration.CommonData;
using NOCIL_VP.Domain.Core.Dtos.Registration.Domestic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Registration
{
    public class DomesticAndImportForm
    {
        public VendorPersonalData_Dto VendorPersonalData { get; set; }
        public VendorOrganizationProfile_Dto VendorOrganizationProfile { get; set; }
        public TechnicalProfile_Dto TechnicalProfile { get; set; }
        public List<Subsideries_Dto> Subsideries { get; set; }
        public List<MajorCustomer_Dto> MajorCustomers { get; set; }
        public CommercialProfile_Dto CommercialProfile { get; set; }
        public Bank_Detail_Dto BankDetail { get; set; }
        public List<Address_Dto> Addresses { get; set; }
        public List<Contact_Dto> Contacts { get; set; }
        public List<VendorBranch_Dto> VendorBranches { get; set; }
        public List<ProprietorOrPartner_Dto> ProprietorOrPartners { get; set; }
        public List<AnnualTurnOver_Dto> AnnualTurnOvers { get; set; }
        public List<NocilRelatedEmployeeDto> NocilRelatedEmployees { get; set; }
    }

}
