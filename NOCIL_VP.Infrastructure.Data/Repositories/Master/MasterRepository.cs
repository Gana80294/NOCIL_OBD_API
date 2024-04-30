using Microsoft.Extensions.Configuration;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Master
{
    public class MasterRepository : IMasterRepository
    {
        private VpContext _dbContext;
        private IConfiguration _config;

        public MasterRepository(VpContext vpContext, IConfiguration config)
        {
            this._dbContext = vpContext;
            this._config = config;
        }


        public List<CompanyCode> GetCompanyCodes()
        {
            var s = this._config.GetConnectionString("Default");
            return this._dbContext.Company_Codes.ToList();
        }

        public List<Department> GetDepartments()
        {
            return this._dbContext.Departments.ToList();
        }

        public List<OrganizationType> GetOrganizationTypes()
        {
            return this._dbContext.Organization_Types.ToList();
        }

        public List<PurchaseOrganization> GetPurchaseOrganization()
        {
            return this._dbContext.Purchase_Organizations.ToList();
        }

        public List<TankerType> GetTankerTypes()
        {
            return this._dbContext.Type_of_Tankers.ToList();
        }

        public List<VendorType> GetVendorTypes()
        {
            return this._dbContext.Type_of_Vendors.ToList();
        }

        public List<CompanyStatus> GetCompanyStatuses()
        {
            return this._dbContext.Company_Statuses.ToList();
        }

        public List<AddressType> GetAddressTypes()
        {
            return this._dbContext.Type_of_Addresses.ToList();
        }

        public List<ContactType> GetContactTypes()
        {
            return this._dbContext.Type_of_Contacts.ToList();
        }
    }
}
