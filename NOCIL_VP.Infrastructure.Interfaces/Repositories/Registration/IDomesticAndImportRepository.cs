using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration
{
    public interface IDomesticAndImportRepository
    {
        Task<bool> SaveDomesticAndImportVendorDetails(DomesticAndImportForm domesticForm, int formId);
        Task<bool> UpdateDomesticAndImportVendorDetails(DomesticAndImportForm domesticForm, int formId);
    }
}
