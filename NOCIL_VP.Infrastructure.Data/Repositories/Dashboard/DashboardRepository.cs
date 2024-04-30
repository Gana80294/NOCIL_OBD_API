using Microsoft.EntityFrameworkCore;
using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Registration;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private VpContext _dbContext;
        
        public DashboardRepository(VpContext context)
        {
            this._dbContext = context;

        }


        public async Task<InitialData> GetInitialData(string employeeId)
        {
            InitialData initialData = new InitialData();
            List<DashboardDto> result = new List<DashboardDto>();

            var initiated = (from form in _dbContext.Forms
                             where form.Created_By == employeeId && form.Status_Id == (int)FormStatusEnum.Initiated
                             select new DashboardDto
                             {
                                 FormId = form.Form_Id,
                                 VendorTypeId = form.Vendor_Type_Id,
                                 VendorType = form.VendorType.Vendor_Type,
                                 Email = form.Vendor_Mail,
                                 Mobile = form.Vendor_Mobile,
                                 Name = form.Vendor_Name,
                                 CreatedOn = form.Created_On,
                                 Status = form.FormStatus.Status
                             }).ToList();
            var pending = (from form in _dbContext.Forms
                           join task in _dbContext.Tasks on form.Form_Id equals task.Form_Id
                           where task.Owner_Id == employeeId && task.Status == "Active"
                           select new DashboardDto
                           {
                               FormId = form.Form_Id,
                               VendorTypeId = form.Vendor_Type_Id,
                               VendorType = form.VendorType.Vendor_Type,
                               Email = form.Vendor_Mail,
                               Mobile = form.Vendor_Mobile,
                               Name = form.Vendor_Name,
                               CreatedOn = form.Created_On,
                               Status = form.FormStatus.Status
                           }).Distinct().ToList();
            var others = (from form in _dbContext.Forms
                          join task in _dbContext.Tasks on form.Form_Id equals task.Form_Id
                          where (task.Owner_Id == employeeId && task.Status != "Active") && (form.Status_Id == (int)FormStatusEnum.Rejected || form.Status_Id == (int)FormStatusEnum.Approved)
                          select new DashboardDto
                          {
                              FormId = form.Form_Id,
                              VendorTypeId = form.Vendor_Type_Id,
                              VendorType = form.VendorType.Vendor_Type,
                              Email = form.Vendor_Mail,
                              Mobile = form.Vendor_Mobile,
                              Name = form.Vendor_Name,
                              CreatedOn = form.Created_On,
                              Status = form.FormStatus.Status
                          }).Distinct().ToList();

            initialData.Data = pending.ToList();
            initialData.Open = initiated.Count();
            initialData.Pending = pending.Count();
            initialData.Approved = others.Count(x => x.Status == FormStatusEnum.Approved.ToString());
            initialData.Rejected = others.Count(x => x.Status == FormStatusEnum.Rejected.ToString());
            return initialData;
        }

        public async Task<List<DashboardDto>> GetInitiatedData(string employeeId)
        {
            var result = (from form in _dbContext.Forms
                          where form.Status_Id == (int)FormStatusEnum.Initiated && form.Created_By == employeeId
                          select new DashboardDto
                          {
                              FormId = form.Form_Id,
                              VendorTypeId = form.Vendor_Type_Id,
                              VendorType = form.VendorType.Vendor_Type,
                              Email = form.Vendor_Mail,
                              Mobile = form.Vendor_Mobile,
                              Name = form.Vendor_Name,
                              CreatedOn = form.Created_On,
                              Status = form.FormStatus.Status
                          }).Distinct().ToList();
            return result;
        }

        public async Task<List<DashboardDto>> GetPendingData(string employeeId)
        {
            var result = (from form in _dbContext.Forms
                          join task in _dbContext.Tasks on form.Form_Id equals task.Form_Id
                          where form.Status_Id == (int)FormStatusEnum.Pending && task.Owner_Id == employeeId && task.Status == "Active"
                          select new DashboardDto
                          {
                              FormId = form.Form_Id,
                              VendorTypeId = form.Vendor_Type_Id,
                              VendorType = form.VendorType.Vendor_Type,
                              Email = form.Vendor_Mail,
                              Mobile = form.Vendor_Mobile,
                              Name = form.Vendor_Name,
                              CreatedOn = form.Created_On,
                              Status = form.FormStatus.Status
                          }).Distinct().ToList();
            return result;
        }

        public async Task<List<DashboardDto>> GetApprovedData(string employeeId)
        {
            var result = (from form in _dbContext.Forms
                          join task in _dbContext.Tasks on form.Form_Id equals task.Form_Id
                          where form.Status_Id == (int)FormStatusEnum.Approved && task.Owner_Id == employeeId
                          select new DashboardDto
                          {
                              FormId = form.Form_Id,
                              VendorTypeId = form.Vendor_Type_Id,
                              VendorType = form.VendorType.Vendor_Type,
                              Email = form.Vendor_Mail,
                              Mobile = form.Vendor_Mobile,
                              Name = form.Vendor_Name,
                              CreatedOn = form.Created_On,
                              Status = form.FormStatus.Status
                          }).Distinct().ToList();
            return result;
        }

        public async Task<List<DashboardDto>> GetRejectedData(string employeeId)
        {
            var result = (from form in _dbContext.Forms
                          join task in _dbContext.Tasks on form.Form_Id equals task.Form_Id
                          where form.Status_Id == (int)FormStatusEnum.Rejected && task.Owner_Id == employeeId
                          select new DashboardDto
                          {
                              FormId = form.Form_Id,
                              VendorTypeId = form.Vendor_Type_Id,
                              VendorType = form.VendorType.Vendor_Type,
                              Email = form.Vendor_Mail,
                              Mobile = form.Vendor_Mobile,
                              Name = form.Vendor_Name,
                              CreatedOn = form.Created_On,
                              Status = form.FormStatus.Status
                          }).Distinct().ToList();
            return result;
        }

        public async Task<InitialData> GetAllData()
        {
            InitialData initialData = new InitialData();
            List<DashboardDto> result = new List<DashboardDto>();

            var res = (from form in _dbContext.Forms
                       select new DashboardDto
                       {
                           FormId = form.Form_Id,
                           VendorTypeId = form.Vendor_Type_Id,
                           VendorType = form.VendorType.Vendor_Type,
                           Email = form.Vendor_Mail,
                           Mobile = form.Vendor_Mobile,
                           Name = form.Vendor_Name,
                           CreatedOn = form.Created_On,
                           Status = form.FormStatus.Status
                       }).ToList();

            initialData.Data = res;
            initialData.Open = res.Count(x=>x.Status == FormStatusEnum.Initiated.ToString());
            initialData.Pending = res.Count(x => x.Status == FormStatusEnum.Pending.ToString());
            initialData.Approved = res.Count(x => x.Status == FormStatusEnum.Approved.ToString());
            initialData.Rejected = res.Count(x => x.Status == FormStatusEnum.Rejected.ToString());
            initialData.SAP = res.Count(x => x.Status == FormStatusEnum.SAP.ToString());

            return initialData;
        }
    
        public async Task<List<DashboardDto>> GetAllInitiatedData()
        {
            var result = (from form in _dbContext.Forms
                             where form.Status_Id == (int)FormStatusEnum.Initiated
                             select new DashboardDto
                             {
                                 FormId = form.Form_Id,
                                 VendorTypeId = form.Vendor_Type_Id,
                                 VendorType = form.VendorType.Vendor_Type,
                                 Email = form.Vendor_Mail,
                                 Mobile = form.Vendor_Mobile,
                                 Name = form.Vendor_Name,
                                 CreatedOn = form.Created_On,
                                 Status = form.FormStatus.Status
                             }).ToList();
            return result;
        }

        public async Task<List<DashboardDto>> GetAllPendingData()
        {
            var result = (from form in _dbContext.Forms
                          where form.Status_Id == (int)FormStatusEnum.Pending
                          select new DashboardDto
                          {
                              FormId = form.Form_Id,
                              VendorTypeId = form.Vendor_Type_Id,
                              VendorType = form.VendorType.Vendor_Type,
                              Email = form.Vendor_Mail,
                              Mobile = form.Vendor_Mobile,
                              Name = form.Vendor_Name,
                              CreatedOn = form.Created_On,
                              Status = form.FormStatus.Status
                          }).ToList();
            return result;
        }

        public async Task<List<DashboardDto>> GetAllApprovedData()
        {
            var result = (from form in _dbContext.Forms
                          where form.Status_Id == (int)FormStatusEnum.Approved
                          select new DashboardDto
                          {
                              FormId = form.Form_Id,
                              VendorTypeId = form.Vendor_Type_Id,
                              VendorType = form.VendorType.Vendor_Type,
                              Email = form.Vendor_Mail,
                              Mobile = form.Vendor_Mobile,
                              Name = form.Vendor_Name,
                              CreatedOn = form.Created_On,
                              Status = form.FormStatus.Status
                          }).ToList();
            return result;
        }

        public async Task<List<DashboardDto>> GetAllRejectedData()
        {
            var result = (from form in _dbContext.Forms
                          where form.Status_Id == (int)FormStatusEnum.Rejected
                          select new DashboardDto
                          {
                              FormId = form.Form_Id,
                              VendorTypeId = form.Vendor_Type_Id,
                              VendorType = form.VendorType.Vendor_Type,
                              Email = form.Vendor_Mail,
                              Mobile = form.Vendor_Mobile,
                              Name = form.Vendor_Name,
                              CreatedOn = form.Created_On,
                              Status = form.FormStatus.Status
                          }).ToList();
            return result;
        }

        public async Task<List<DashboardDto>> GetAllSAPData()
        {
            var result = (from form in _dbContext.Forms
                          where form.Status_Id == (int)FormStatusEnum.SAP
                          select new DashboardDto
                          {
                              FormId = form.Form_Id,
                              VendorTypeId = form.Vendor_Type_Id,
                              VendorType = form.VendorType.Vendor_Type,
                              Email = form.Vendor_Mail,
                              Mobile = form.Vendor_Mobile,
                              Name = form.Vendor_Name,
                              CreatedOn = form.Created_On,
                              Status = form.FormStatus.Status
                          }).ToList();
            return result;
        }

    }
}
