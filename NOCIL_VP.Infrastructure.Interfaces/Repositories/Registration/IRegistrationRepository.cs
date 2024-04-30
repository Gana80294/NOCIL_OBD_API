using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Registration.Reason;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration
{
    public interface IRegistrationRepository:IRepository<Form>
    {
        Task<ResponseMessage> InitiateRegistration(FormDto formDto);
        Task<ResponseMessage> SubmitForm(FormSubmitTemplate formData);
        Task<ResponseMessage> ApproveForm(ApprovalDto approvalDto);
        Task<ResponseMessage> RejectForm(RejectDto rejectDto);
        Task<ResponseMessage> UpdateForm(FormSubmitTemplate formData);
        DashboardDto GetSingleFormData(int form_Id);
        ReasonDto GetRejectedReasons(int form_Id);
    }
}
