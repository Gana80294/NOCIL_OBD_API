using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Dtos.Registration.Evaluation;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration.Evaluation;

namespace NOCIL_VP.API.Controllers.Registration.Evaluation
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class VendorGradeController : ControllerBase
    {
        private readonly IVendorGradeRepository _vendorGradeRepository;

        public VendorGradeController(IVendorGradeRepository vendorGradeRepository)
        {
            _vendorGradeRepository = vendorGradeRepository;
        }


        [HttpGet]
        [Authorize(Policy = "AdminBuyerPolicy")]
        public IActionResult GetVendorGradeById(int formId)
        {
            return Ok(this._vendorGradeRepository.GetVendorGradeById(formId));
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddVendorGrade(VendorGradeDto grade)
        {
            return Ok(await this._vendorGradeRepository.AddVendorGrade(grade));
        }

        [HttpPost]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateVendorGrade(VendorGradeDto grade)
        {
            return Ok(await this._vendorGradeRepository.UpdateVendorGrade(grade));
        }
    }
}
