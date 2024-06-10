using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;

namespace NOCIL_VP.API.Controllers.Registration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private IRegistrationRepository _registrationRepository;
        private GSTHelper _gstHelper;
        public RegistrationController(IRegistrationRepository registrationRepository, GSTHelper gstHelper)
        {
            this._registrationRepository = registrationRepository;
            _gstHelper = gstHelper;
        }

        [HttpPost]
        [Authorize(Policy = "POPolicy")]
        public async Task<IActionResult> Initiate(FormDto formDto)
        {
            return Ok(await _registrationRepository.InitiateRegistration(formDto));
        }

        [HttpPost]
        [Authorize(Policy = "VendorPolicy")]
        public async Task<IActionResult> Submit(FormSubmitTemplate formData)
        {
            return Ok(await _registrationRepository.SubmitForm(formData));
        }

        [HttpPost]
        [Authorize(Policy = "BuyerPolicy")]
        public async Task<IActionResult> Approve(ApprovalDto approvalDto)
        {
            return Ok(await _registrationRepository.ApproveForm(approvalDto));
        }

        [HttpPost]
        [Authorize(Policy = "BuyerPolicy")]
        public async Task<IActionResult> Reject(RejectDto rejectDto)
        {
            return Ok(await _registrationRepository.RejectForm(rejectDto));
        }

        [HttpPost]
        [Authorize(Policy = "VendorPolicy")]
        public async Task<IActionResult> Update(FormSubmitTemplate formData)
        {
            return Ok(await _registrationRepository.UpdateForm(formData));
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetSingleFormData(int form_Id)
        {
            return Ok(_registrationRepository.GetSingleFormData(form_Id));
        }

        [HttpGet]
        [Authorize(Policy = "NonAdminPolicy")]
        public IActionResult GetRejectedReasons(int form_Id)
        {
            return Ok(_registrationRepository.GetRejectedReasons(form_Id));
        }

        [HttpGet]
        [Authorize(Policy = "VendorPolicy")]
        public async Task<IActionResult> GetGstDetails(string gstin)
        {
            var data = await _gstHelper.GetGstDetails(gstin);
            if (data != null && data.status_code == 1)
            {
                return Ok(_gstHelper.FormAddressData(JsonConvert.DeserializeObject<IrisApiSuccessResponse>(data.ToString())));
            }
            return BadRequest(ResponseWritter.WriteErrorResponse("Unable to get the address details"));
        }
    }
}
