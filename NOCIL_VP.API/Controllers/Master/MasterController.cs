using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;

namespace NOCIL_VP.API.Controllers.Master
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
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

        [HttpGet]
        public IActionResult GetTitle()
        {
            return Ok(this._masterRepository.GetTitle());
        }

        [HttpGet]
        public IActionResult GetReconciliationAccounts()
        {
            return Ok(this._masterRepository.GetReconciliationAccounts());
        }

        [HttpGet]
        public IActionResult GetTaxBases()
        {
            return Ok(this._masterRepository.GetTaxBases());
        }

        [HttpGet]
        public IActionResult GetIndustry()
        {
            return Ok(this._masterRepository.GetIndustry());
        }

        [HttpGet]
        public IActionResult GetIncoterms()
        {
            return Ok(this._masterRepository.GetIncoterms());
        }

        [HttpGet]
        public IActionResult GetSchemaGroups()
        {
            return Ok(this._masterRepository.GetSchemaGroups());
        }

        [HttpGet]
        public IActionResult GetGSTVenClass()
        {
            return Ok(this._masterRepository.GetGSTVenClass());
        }

        [HttpGet]
        public IActionResult GetCountry()
        {
            return Ok(this._masterRepository.GetCountry());
        }

        [HttpGet]
        public IActionResult GetRegionByCompanyCode()
        { 
            return Ok(this._masterRepository.GetRegionByCompanyCode());
        }
    }
}
