using NOCIL_VP.Domain.Core.Dtos.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Reports
{
    public interface IReportRepository
    {
        Task<List<VendorMasterDto>> GetAllVendorsByType(bool type);
        Task<List<VendorMasterDto>> GetAllTransportVendors();
        Task<List<VendorMasterDto>> SearchAllVendors(VendorReportDto reportDto);

        void CreateFolder();
    }
}
