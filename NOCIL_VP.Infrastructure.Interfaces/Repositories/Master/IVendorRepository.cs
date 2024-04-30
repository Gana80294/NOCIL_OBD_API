using NOCIL_VP.Domain.Core.Dtos.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Master
{
    public interface IVendorRepository
    {
        Task<List<VendorMasterDto>> GetAllVendorsByType(bool type);
        Task<List<VendorMasterDto>> GetAllTransportVendors();
    }
}
