using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration.UpdateForm
{
    public interface IUpdateFormRepository
    {
        Task<bool> UpdateNocilRelatedEmployees(List<NocilRelatedEmployee> entities, int formId);
        Task<bool> UpdateAddress(List<Address> entities, int formId);
        Task<bool> UpdateContacts(List<Contact> entities, int formId);
        Task<bool> UpdateVendorBranches(List<VendorBranch> entities, int formId);
        Task<bool> UpdateProprietors(List<ProprietorOrPartner> entities, int formId);
        Task<bool> UpdateAnnualTurnOvers(List<AnnualTurnOver> entities, int formId);
        Task<bool> UpdateSubsideries(List<Subsideries> entities, int formId);
        Task<bool> UpdateMajorCustomers(List<MajorCustomer> entities, int formId);
    }
}
