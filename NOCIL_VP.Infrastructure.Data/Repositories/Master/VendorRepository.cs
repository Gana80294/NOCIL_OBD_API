using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Master
{
    public class VendorRepository : IVendorRepository
    {
        private VpContext _dbContext;
        public VendorRepository(VpContext vpContext)
        {
            this._dbContext = vpContext;
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
                               Vendor_Name =  tb1.Organization_Name,
                               Vendor_Mobile = form.Vendor_Mobile,
                               Vendor_Mail = form.Vendor_Mail,
                               Vendor_Code = form.Vendor_Code,
                               Vendor_Type = form.VendorType.Vendor_Type,
                               Form_Id = form.Form_Id,
                               VT_Id = form.Vendor_Type_Id
                           }).ToList();
                return res;
            }
            catch(Exception ex)
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
    }
}
