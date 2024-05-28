using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Dashboard;

namespace NOCIL_VP.API.Controllers.Dashboard
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private IDashboardRepository _dashboardRepository;
        private EmailHelper _email;

        public DashboardController(IDashboardRepository dashboardRepository,EmailHelper mail)
        {
            this._dashboardRepository = dashboardRepository;
            _email = mail;
        }

        #region Others Dashboard
        [HttpGet]
        [Authorize(Policy = "BuyerPolicy")]
        public async Task<IActionResult> GetInitialData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetInitialData(employeeId));
        }

        [HttpGet]
        [Authorize(Policy = "BuyerPolicy")]
        public async Task<IActionResult> GetInitiatedData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetInitiatedData(employeeId));
        }

        [HttpGet]
        [Authorize(Policy = "BuyerPolicy")]
        public async Task<IActionResult> GetPendingData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetPendingData(employeeId));
        }

        [HttpGet]
        [Authorize(Policy = "BuyerPolicy")]
        public async Task<IActionResult> GetApprovedData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetApprovedData(employeeId));
        }

        [HttpGet]
        [Authorize(Policy = "BuyerPolicy")]
        public async Task<IActionResult> GetRejectedData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetRejectedData(employeeId));
        }

        #endregion

        #region Admin Dashboard
        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllData()
        {
            return Ok(await this._dashboardRepository.GetAllData());
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllInitiatedData()
        {
            return Ok(await this._dashboardRepository.GetAllInitiatedData());
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllPendingData()
        {
            return Ok(await this._dashboardRepository.GetAllPendingData());
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllApprovedData()
        {
            return Ok(await this._dashboardRepository.GetAllApprovedData());
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllRejectedData()
        {
            return Ok(await this._dashboardRepository.GetAllRejectedData());
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllSAPData()
        {
            return Ok(await this._dashboardRepository.GetAllSAPData());
        }
        #endregion


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SendTestMail()
        {
            SendMail sendMail = new SendMail()
            {
                Form_Id = 0,
                Vendor_Type_Id = 0,
                Username = "Gana",
                ToEmail = "gana@exalca.com"
            };
            await this._email.SendMailToVendors(sendMail);
            return Ok(true);
        }
    }
}
