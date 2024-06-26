﻿using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Master
{
    public interface IMasterRepository
    {
        public List<CompanyCode> GetCompanyCodes();
        public List<Department> GetDepartments();
        public List<OrganizationType> GetOrganizationTypes();
        public List<PurchaseOrganization> GetPurchaseOrganization();
        public List<TankerType> GetTankerTypes();
        public List<VendorType> GetVendorTypes();
        public List<CompanyStatus> GetCompanyStatuses();
        public List<AddressType> GetAddressTypes();
        public List<ContactType> GetContactTypes();

        public List<Title> GetTitle();
        public List<ReconciliationAccount> GetReconciliationAccounts();
        public List<TaxBase> GetTaxBases();
        public List<Industry> GetIndustry();
        public List<Incoterms> GetIncoterms();
        public List<SchemaGroup> GetSchemaGroups();
        public List<GSTVenClass> GetGSTVenClass();
        public List<Country> GetCountry();
        public List<Region> GetRegionByCompanyCode();
        public List<VendorAccountGroup> GetVendorAccountGroup();

    }
}
