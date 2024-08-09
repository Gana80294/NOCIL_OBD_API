using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Registration;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Reports;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Reports
{
    public class ReportRepository : IReportRepository
    {
        private VpContext _dbContext;
        public ReportRepository(VpContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<VendorMasterDto>> GetAllVendorsByType(bool type)
        {
            try
            {
                var res = (from profile in _dbContext.Technical_Profile
                           where profile.Is_ISO_Certified == type
                           join form in _dbContext.Forms
                           on profile.Form_Id equals form.Form_Id
                           join tb1 in _dbContext.Vendor_Personal_Data
                           on form.Form_Id equals tb1.Form_Id
                           select new VendorMasterDto
                           {
                               Vendor_Name = tb1.Organization_Name,
                               Vendor_Mobile = form.Vendor_Mobile,
                               Vendor_Mail = form.Vendor_Mail,
                               Vendor_Code = form.Vendor_Code,
                               Vendor_Type = form.VendorType.Vendor_Type,
                               Form_Id = form.Form_Id,
                               VT_Id = form.Vendor_Type_Id
                           }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<VendorMasterDto>> GetAllTransportVendors()
        {
            try
            {
                var res = (from trasport in _dbContext.Transport_Vendor_Personal_Data
                           join form in _dbContext.Forms
                           on trasport.Form_Id equals form.Form_Id
                           select new VendorMasterDto
                           {
                               Vendor_Name = trasport.Name_of_Transporter,
                               Vendor_Mobile = form.Vendor_Mobile,
                               Vendor_Mail = form.Vendor_Mail,
                               Vendor_Code = form.Vendor_Code,
                               Vendor_Type = form.VendorType.Vendor_Type,
                               Form_Id = form.Form_Id,
                               VT_Id = form.Vendor_Type_Id
                           }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<VendorMasterDto>> SearchAllVendors(VendorReportDto reportDto)
        {
            var isVendorType = reportDto.Type_of_Vendor.Count > 0;

            var res = (from form in _dbContext.Forms
                       where (!isVendorType || reportDto.Type_of_Vendor.Any(x => x == form.Vendor_Type_Id))
                       && !string.IsNullOrEmpty(form.Vendor_Code)
                       select form).AsQueryable();
            if (reportDto.Vendor_Category.Count > 0)
            {
                var isIso = reportDto.Vendor_Category.Contains("ISO");
                var isNonIso = reportDto.Vendor_Category.Contains("Non-ISO");

                if (!isIso || !isNonIso)
                {
                    if (isIso)
                    {
                        res = (from form in res
                               join tech in _dbContext.Technical_Profile on form.Form_Id equals tech.Form_Id
                               where tech.Is_ISO_Certified
                               select form).AsQueryable();
                    }
                    else
                    {
                        res = (from form in res
                               join tech in _dbContext.Technical_Profile on form.Form_Id equals tech.Form_Id
                               where !tech.Is_ISO_Certified
                               select form).AsQueryable();
                    }
                }
            }
            if (!string.IsNullOrEmpty(reportDto.Country_Code))
            {
                var isState = reportDto.Region_Id.HasValue;

                res = (from form in res
                       join address in _dbContext.Addresses on form.Form_Id equals address.Form_Id
                       where address.Country_Code == reportDto.Country_Code &&
                       (!isState || address.Region_Id == reportDto.Region_Id)
                       select form).AsQueryable();
            }

            if (reportDto.Type_of_Organization.Count > 0)
            {
                res = (from form in res
                       join org in _dbContext.Vendor_Organization_Profile on form.Form_Id equals org.Form_Id
                       where reportDto.Type_of_Organization.Any(x => x == org.Type_of_Org_Id)
                       select form).AsQueryable();
            }

            var result = await (from v in res
                                join vType in _dbContext.Type_of_Vendors on v.Vendor_Type_Id equals vType.Id
                                select new VendorMasterDto
                                {
                                    Vendor_Name = v.Vendor_Name,
                                    Vendor_Mobile = v.Vendor_Mobile,
                                    Vendor_Mail = v.Vendor_Mail,
                                    Vendor_Code = v.Vendor_Code,
                                    Vendor_Type = vType.Vendor_Type,
                                    Form_Id = v.Form_Id,
                                    VT_Id = v.Vendor_Type_Id
                                }).ToListAsync();

            return result;
        }


        //public async Task<List<VendorMasterDto>> SearchAllVendors(VendorReportDto reportDto)
        //{
        //    try
        //    {
        //        var res = from form in _dbContext.Forms
        //                  join techProfile in _dbContext.Technical_Profile
        //                      on form.Form_Id equals techProfile.Form_Id into techProfileGroup
        //                  from techProfile in techProfileGroup.DefaultIfEmpty()

        //                  join address in _dbContext.Addresses
        //                      on form.Form_Id equals address.Form_Id into addressGroup
        //                  from address in addressGroup.DefaultIfEmpty()

        //                  join vendorOrg in _dbContext.Vendor_Organization_Profile
        //                      on form.Form_Id equals vendorOrg.Form_Id into vendorOrgGroup
        //                  from vendorOrg in vendorOrgGroup.DefaultIfEmpty()

        //                  join orgTypes in _dbContext.Organization_Types
        //                      on vendorOrg.Type_of_Org_Id equals orgTypes.Id into orgTypesGroup
        //                  from orgTypes in orgTypesGroup.DefaultIfEmpty()

        //                  join type in _dbContext.Type_of_Vendors
        //                      on form.Vendor_Type_Id equals type.Id into typeGroup
        //                  from type in typeGroup.DefaultIfEmpty()

        //                  join transportData in _dbContext.Transport_Vendor_Personal_Data
        //                      on form.Form_Id equals transportData.Form_Id into transportDataGroup
        //                  from transportData in transportDataGroup.DefaultIfEmpty()

        //                  where techProfile != null
        //                  select new
        //                  {
        //                      form,
        //                      techProfile,
        //                      address,
        //                      orgTypes,
        //                      type,
        //                      transportData
        //                  };

        //        if (reportDto.Vendor_Category.HasValue)
        //        {
        //            bool isISOValue = reportDto.Vendor_Category.Value;
        //            res = res.Where(v => v.techProfile.Is_ISO_Certified == isISOValue);
        //        }

        //        if (!string.IsNullOrEmpty(reportDto.Country_Code))
        //        {
        //            res = res.Where(v => v.address != null && v.address.Country_Code == reportDto.Country_Code);
        //        }

        //        if (reportDto.Region_Id.HasValue && reportDto.Region_Id > 0)
        //        {
        //            res = res.Where(v => v.address != null && v.address.Region_Id == reportDto.Region_Id);
        //        }

        //        if (reportDto.Category_Vendor != null && reportDto.Category_Vendor.Any())
        //        {
        //            res = res.Where(v => v.orgTypes != null && reportDto.Category_Vendor.Contains(v.orgTypes.Id));
        //        }

        //        if (reportDto.Type_of_Vendor != null && reportDto.Type_of_Vendor.Any())
        //        {
        //            res = res.Where(v => v.type != null && reportDto.Type_of_Vendor.Contains(v.type.Id));
        //        }

        //        if (reportDto.ISO_Due_Date.HasValue)
        //        {
        //            res = res.Where(v => v.form.Created_On <= reportDto.ISO_Due_Date.Value);
        //        }

        //        if (reportDto.Nicerglobe_Registration.HasValue)
        //        {
        //            bool nicerglobeStatus = reportDto.Nicerglobe_Registration.Value;
        //            res = res.Where(v => v.transportData != null && v.transportData.Nicerglobe_Registration_Status == nicerglobeStatus);
        //        }

        //        var result = await res
        //                        .Select(v => new VendorMasterDto
        //                        {
        //                            Vendor_Name = v.form.Vendor_Name,
        //                            Vendor_Mobile = v.form.Vendor_Mobile,
        //                            Vendor_Mail = v.form.Vendor_Mail,
        //                            Vendor_Code = v.form.Vendor_Code,
        //                            Vendor_Type = v.form.VendorType.Vendor_Type,
        //                            Form_Id = v.form.Form_Id,
        //                            VT_Id = v.form.Vendor_Type_Id
        //                        }).ToListAsync();

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




        public void CreateFolder()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "TempFolder";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(file);
                }
            }
        }
    }
}
