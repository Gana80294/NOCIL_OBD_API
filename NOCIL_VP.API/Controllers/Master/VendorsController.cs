using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;

namespace NOCIL_VP.API.Controllers.Master
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private IVendorRepository _vendorRepository;
        public VendorsController(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVendorsByType(bool type)
        {
            return Ok(await _vendorRepository.GetAllVendorsByType(type));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransportVendors()
        {
            return Ok(await _vendorRepository.GetAllTransportVendors());
        }
    }
}
