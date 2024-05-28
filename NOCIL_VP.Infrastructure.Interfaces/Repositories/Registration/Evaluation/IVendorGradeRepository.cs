using NOCIL_VP.Domain.Core.Dtos.Registration.Evaluation;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities.Registration.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration.Evaluation
{
    public interface IVendorGradeRepository : IRepository<VendorGrade>
    {
        Task<ResponseMessage> AddVendorGrade(VendorGradeDto vendorGrade);
        Task<ResponseMessage> UpdateVendorGrade(VendorGradeDto vendorGrade);
        VendorGrade GetVendorGradeById(int formId);
    }
}
