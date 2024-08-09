using NOCIL_VP.Domain.Core.Dtos.Registration.CommonData;
using NOCIL_VP.Domain.Core.Dtos.Registration.Transport;

namespace NOCIL_VP.Domain.Core.Dtos.Registration
{
    public class TransportForm
    {
        public TransportVendorPersonalData_Dto TransportVendorPersonalData { get; set; }
        public List<Address_Dto> Addresses { get; set; }
        public List<Contact_Dto> Contacts { get; set; }
        public List<TankerDetail_Dto> TankerDetails { get; set; }
        public List<VehicleDetails_Dto> VehicleDetails { get; set; }
        public Bank_Detail_Dto BankDetail { get; set; }
        public CommercialProfile_Dto CommercialProfile { get; set; }
        public List<VendorBranch_Dto> VendorBranches { get; set; }
    }
}
