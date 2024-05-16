using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;

namespace NOCIL_VP.API.Controllers.Master
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }



        [HttpPost]
        public async Task<IActionResult> AddUser(UserDto user)
        {
            var result = await _userRepository.AddUser(user);
            if (result)
            {
                return Ok(ResponseWritter.WriteSuccessResponse("User added Successfully."));
            }
            return BadRequest(ResponseWritter.WriteErrorResponse("Something went wrong."));
        }

        [HttpPost]
        public async Task<IActionResult> SoftDeleteUser(UserDto user)
        {
            var result = await _userRepository.SoftDeleteUser(user);
            if (result)
            {
                return Ok(ResponseWritter.WriteSuccessResponse("User deleted Successfully."));
            }
            return BadRequest(ResponseWritter.WriteErrorResponse("Something went wrong."));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userRepository.GetUsers());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            var result = await _userRepository.UpdateUser(userDto);
            if (result)
            {
                return Ok(ResponseWritter.WriteSuccessResponse("User updated Successfully."));
            }
            return BadRequest(ResponseWritter.WriteErrorResponse("Something went wrong."));
        }
    }
}
