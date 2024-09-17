using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Infrastructure.Data.Repositories.Registration;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;

namespace NOCIL_VP.API.Controllers.Registration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EditRequestController : ControllerBase
    {
        private IEditRequestRepository _editReqRepository;
        public EditRequestController(IEditRequestRepository editReqRepository)
        {
            this._editReqRepository = editReqRepository;
        }

        [HttpPost]
        public async Task<IActionResult> RequestForEdit(RequestForEdit data)
        {
            return Ok(await _editReqRepository.RequestForEdit(data));
        }

        [HttpPost]
        public async Task<IActionResult> AcceptEditRequest(int formId)
        {
            return Ok(await _editReqRepository.AcceptEditRequest(formId));
        }

        [HttpPost]
        public async Task<IActionResult> RejectEditRequest(RejectDto reject)
        {
            return Ok(await _editReqRepository.RejectEditRequest(reject));
        }

        [HttpGet]
        public async Task<IActionResult> GetEditRequestData(string employeeId)
        {
            return Ok(await this._editReqRepository.GetEditRequestData(employeeId));
        }

        [HttpPost]
        public async Task<IActionResult> Update(FormSubmitTemplate formData)
        {
            return Ok(await _editReqRepository.UpdateForm(formData));
        }

        [HttpPost]
        public async Task<IActionResult> Approve(ApprovalDto approvalDto)
        {
            return Ok(await _editReqRepository.ApproveForm(approvalDto));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(RejectDto rejectDto)
        {
            return Ok(await _editReqRepository.RejectForm(rejectDto));
        }

        [HttpGet]
        public IActionResult GetEditRejectedReason(int form_Id)
        {
            return Ok(_editReqRepository.GetEditRejectedReason(form_Id));
        }


    }
}
