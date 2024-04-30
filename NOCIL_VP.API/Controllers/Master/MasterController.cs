using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;

namespace NOCIL_VP.API.Controllers.Master
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private IMasterRepository _masterRepository;
        public MasterController(IMasterRepository masterRepository)
        {
            this._masterRepository = masterRepository;
        }

        [HttpGet]
        public IActionResult GetCompanyCodes()
        {
            return Ok(this._masterRepository.GetCompanyCodes());
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            return Ok(this._masterRepository.GetDepartments());
        }

        [HttpGet]
        public IActionResult GetOrganizationTypes()
        {
            return Ok(this._masterRepository.GetOrganizationTypes());
        }

        [HttpGet]
        public IActionResult GetPurchaseOrganization()
        {
            return Ok(this._masterRepository.GetPurchaseOrganization());
        }

        [HttpGet]
        public IActionResult GetTankerTypes()
        {
            return Ok(this._masterRepository.GetTankerTypes());
        }

        [HttpGet]
        public IActionResult GetVendorTypes()
        {
            return Ok(this._masterRepository.GetVendorTypes());
        }

        [HttpGet]
        public IActionResult GetCompanyStatuses()
        {
            return Ok(this._masterRepository.GetCompanyStatuses());
        }

        [HttpGet]
        public IActionResult GetAddressTypes()
        {
            return Ok(this._masterRepository.GetAddressTypes());
        }

        [HttpGet]
        public IActionResult GetContactTypes()
        {
            return Ok(this._masterRepository.GetContactTypes());
        }
    }
}
