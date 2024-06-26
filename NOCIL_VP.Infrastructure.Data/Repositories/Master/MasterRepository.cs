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

        public MasterRepository(VpContext vpContext)
        {
            this._dbContext = vpContext;
        }


        public List<CompanyCode> GetCompanyCodes()
        {
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

        public List<Title> GetTitle()
        {
            return this._dbContext.Titles.ToList();
        }

        public List<ReconciliationAccount> GetReconciliationAccounts()
        {
            return this._dbContext.ReconciliationAccounts.ToList();
        }

        public List<TaxBase> GetTaxBases()
        {
            return this._dbContext.TaxBases.ToList();
        }

        public List<Industry> GetIndustry()
        {
            return this._dbContext.Industry.ToList();
        }

        public List<Incoterms> GetIncoterms()
        {
            return this._dbContext.Incoterms.ToList();
        }

        public List<SchemaGroup> GetSchemaGroups()
        {
            return this._dbContext.SchemaGroups.ToList();
        }

        public List<GSTVenClass> GetGSTVenClass()
        {
            return this._dbContext.GSTVenClass.ToList();
        }

        public List<Country> GetCountry()
        {
            return this._dbContext.Country.ToList();
        }

        public List<Region> GetRegionByCompanyCode()
        {
            return this._dbContext.Region.ToList();
        }

        public List<VendorAccountGroup> GetVendorAccountGroup()
        {
            return this._dbContext.VendorAccountGroups.ToList();
        }

       
    }
}
