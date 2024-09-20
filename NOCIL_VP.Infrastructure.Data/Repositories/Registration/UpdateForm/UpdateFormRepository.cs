using Microsoft.EntityFrameworkCore;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration.UpdateForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Registration.UpdateForm
{
    public class UpdateFormRepository : IUpdateFormRepository
    {
        private readonly VpContext _dbContext;

        public UpdateFormRepository(VpContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> UpdateNocilRelatedEmployees(List<NocilRelatedEmployee> entities, int formId)
        {
            var allRecords = _dbContext.NocilRelatedEmployees.AsNoTracking().Where(x => x.Form_Id == formId).ToList();
            var existing = entities.Where(x => allRecords.Any(y => y.Id == x.Id)).ToList();
            var newRecords = entities.Where(x => x.Id == 0).ToList();
            var delete = allRecords.Where(x => existing.All(y => y.Id != x.Id)).ToList();

            if (delete.Count > 0)
            {
                _dbContext.NocilRelatedEmployees.RemoveRange(delete);
            }
            if (existing.Count > 0)
            {
                _dbContext.NocilRelatedEmployees.UpdateRange(existing);
            }
            if (newRecords.Count > 0)
            {
                _dbContext.NocilRelatedEmployees.AddRange(newRecords);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAddress(List<Address> entities, int formId)
        {
            var allRecords = _dbContext.Addresses.AsNoTracking().Where(x => x.Form_Id == formId).ToList();
            var existing = entities.Where(x => allRecords.Any(y => y.Address_Id == x.Address_Id)).ToList();
            var newRecords = entities.Where(x => x.Address_Id == 0).ToList();
            var delete = allRecords.Where(x => existing.All(y => y.Address_Id != x.Address_Id)).ToList();

            if (delete.Count > 0)
            {
                _dbContext.Addresses.RemoveRange(delete);
            }
            if (existing.Count > 0)
            {
                _dbContext.Addresses.UpdateRange(existing);
            }
            if (newRecords.Count > 0)
            {
                _dbContext.Addresses.AddRange(newRecords);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateContacts(List<Contact> entities, int formId)
        {
            var allRecords = _dbContext.Contacts.AsNoTracking().Where(x => x.Form_Id == formId).ToList();
            var existing = entities.Where(x => allRecords.Any(y => y.Contact_Id == x.Contact_Id)).ToList();
            var newRecords = entities.Where(x => x.Contact_Id == 0).ToList();
            var delete = allRecords.Where(x => existing.All(y => y.Contact_Id != x.Contact_Id)).ToList();

            if (delete.Count > 0)
            {
                _dbContext.Contacts.RemoveRange(delete);
            }
            if (existing.Count > 0)
            {
                _dbContext.Contacts.UpdateRange(existing);
            }
            if (newRecords.Count > 0)
            {
                _dbContext.Contacts.AddRange(newRecords);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateVendorBranches(List<VendorBranch> entities, int formId)
        {
            var allRecords = _dbContext.VendorBranches.AsNoTracking().Where(x => x.Form_Id == formId).ToList();
            var existing = entities.Where(x => allRecords.Any(y => y.Branch_Id == x.Branch_Id)).ToList();
            var newRecords = entities.Where(x => x.Branch_Id == 0).ToList();
            var delete = allRecords.Where(x => existing.All(y => y.Branch_Id != x.Branch_Id)).ToList();

            if (delete.Count > 0)
            {
                _dbContext.VendorBranches.RemoveRange(delete);
            }
            if (existing.Count > 0)
            {
                _dbContext.VendorBranches.UpdateRange(existing);
            }
            if (newRecords.Count > 0)
            {
                _dbContext.VendorBranches.AddRange(newRecords);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProprietors(List<ProprietorOrPartner> entities, int formId)
        {
            var allRecords = _dbContext.Proprietor_or_Partners.AsNoTracking().Where(x => x.Form_Id == formId).ToList();
            var existing = entities.Where(x => allRecords.Any(y => y.Id == x.Id)).ToList();
            var newRecords = entities.Where(x => x.Id == 0).ToList();
            var delete = allRecords.Where(x => existing.All(y => y.Id != x.Id)).ToList();

            if (delete.Count > 0)
            {
                _dbContext.Proprietor_or_Partners.RemoveRange(delete);
            }
            if (existing.Count > 0)
            {
                _dbContext.Proprietor_or_Partners.UpdateRange(existing);
            }
            if (newRecords.Count > 0)
            {
                _dbContext.Proprietor_or_Partners.AddRange(newRecords);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAnnualTurnOvers(List<AnnualTurnOver> entities, int formId)
        {
            var allRecords = _dbContext.Annual_TurnOver.AsNoTracking().Where(x => x.Form_Id == formId).ToList();
            var existing = entities.Where(x => allRecords.Any(y => y.TurnOver_Id == x.TurnOver_Id)).ToList();
            var newRecords = entities.Where(x => x.TurnOver_Id == 0).ToList();
            var delete = allRecords.Where(x => existing.All(y => y.TurnOver_Id != x.TurnOver_Id)).ToList();

            if (delete.Count > 0)
            {
                _dbContext.Annual_TurnOver.RemoveRange(delete);
            }
            if (existing.Count > 0)
            {
                _dbContext.Annual_TurnOver.UpdateRange(existing);
            }
            if (newRecords.Count > 0)
            {
                _dbContext.Annual_TurnOver.AddRange(newRecords);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSubsideries(List<Subsideries> entities, int formId)
        {
            var allRecords = _dbContext.Subsideries.AsNoTracking().Where(x => x.Form_Id == formId).ToList();
            var existing = entities.Where(x => allRecords.Any(y => y.Id == x.Id)).ToList();
            var newRecords = entities.Where(x => x.Id == 0).ToList();
            var delete = allRecords.Where(x => existing.All(y => y.Id != x.Id)).ToList();

            if (delete.Count > 0)
            {
                _dbContext.Subsideries.RemoveRange(delete);
            }
            if (existing.Count > 0)
            {
                _dbContext.Subsideries.UpdateRange(existing);
            }
            if (newRecords.Count > 0)
            {
                _dbContext.Subsideries.AddRange(newRecords);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMajorCustomers(List<MajorCustomer> entities, int formId)
        {
            var allRecords = _dbContext.MajorCustomers.AsNoTracking().Where(x => x.Form_Id == formId).ToList();
            var existing = entities.Where(x => allRecords.Any(y => y.Id == x.Id)).ToList();
            var newRecords = entities.Where(x => x.Id == 0).ToList();
            var delete = allRecords.Where(x => existing.All(y => y.Id != x.Id)).ToList();

            if (delete.Count > 0)
            {
                _dbContext.MajorCustomers.RemoveRange(delete);
            }
            if (existing.Count > 0)
            {
                _dbContext.MajorCustomers.UpdateRange(existing);
            }
            if (newRecords.Count > 0)
            {
                _dbContext.MajorCustomers.AddRange(newRecords);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
