using NOCIL_VP.Domain.Core.Dtos.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration
{
    public interface IServiceRepository
    {
        Task<bool> SaveServiceVendorDetails(ServiceForm serviceForm, int formId);
        Task<bool> UpdateServiceVendorDetails(ServiceForm serviceForm, int formId);
    }
}
