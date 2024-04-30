using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;

namespace NOCIL_VP.API.Controllers.Master
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleRepository _roleRepository;
        public RolesController(IRoleRepository roleRepository)
        {
            this._roleRepository = roleRepository;
        }

        [HttpPost]
        public IActionResult AddRole(Role role)
        {
            return Ok(_roleRepository.AddRole(role));
        }

        [HttpPut]
        public IActionResult UpdateRole(Role role)
        {
            return Ok(_roleRepository.UpdateRole(role));
        }

        [HttpDelete]
        public IActionResult RemoveRole(Role role)
        {
            return Ok(_roleRepository.DeleteRole(role));
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            return Ok(_roleRepository.GetRoles());
        }
    }
}
