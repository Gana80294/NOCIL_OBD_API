using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Registration.Reason;
using NOCIL_VP.Domain.Core.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration
{
    public interface IEditRequestRepository
    {
        Task<ResponseMessage> RequestForEdit(RequestForEdit data);
        Task<ResponseMessage> AcceptEditRequest(int formId);
        Task<ResponseMessage> RejectEditRequest(RejectDto reject);
        Task<List<DashboardDto>> GetEditRequestData(string employeeId);
        Task<ResponseMessage> UpdateForm(FormSubmitTemplate formData);
        Task<ResponseMessage> RejectForm(RejectDto rejectDto);
        Task<ResponseMessage> ApproveForm(ApprovalDto approvalDto);
        List<ReasonDetailDto> GetEditRejectedReason(int formId);
    }
}
