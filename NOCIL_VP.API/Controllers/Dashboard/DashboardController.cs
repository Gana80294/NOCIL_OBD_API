using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Dashboard;

namespace NOCIL_VP.API.Controllers.Dashboard
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private IDashboardRepository _dashboardRepository;
        private EmailHelper _email;

        public DashboardController(IDashboardRepository dashboardRepository, IConfiguration config)
        {
            this._dashboardRepository = dashboardRepository;
            _email = new EmailHelper(config);
        }

        #region Others Dashboard
        [HttpGet]
        public async Task<IActionResult> GetInitialData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetInitialData(employeeId));
        }

        [HttpGet]
        public async Task<IActionResult> GetInitiatedData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetInitiatedData(employeeId));
        }

        [HttpGet]
        public async Task<IActionResult> GetPendingData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetPendingData(employeeId));
        }

        [HttpGet]
        public async Task<IActionResult> GetApprovedData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetApprovedData(employeeId));
        }

        [HttpGet]
        public async Task<IActionResult> GetRejectedData(string employeeId)
        {
            return Ok(await this._dashboardRepository.GetRejectedData(employeeId));
        }

        #endregion

        #region Admin Dashboard
        [HttpGet]
        public async Task<IActionResult> GetAllData()
        {
            return Ok(await this._dashboardRepository.GetAllData());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInitiatedData()
        {
            return Ok(await this._dashboardRepository.GetAllInitiatedData());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPendingData()
        {
            return Ok(await this._dashboardRepository.GetAllPendingData());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllApprovedData()
        {
            return Ok(await this._dashboardRepository.GetAllApprovedData());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRejectedData()
        {
            return Ok(await this._dashboardRepository.GetAllRejectedData());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSAPData()
        {
            return Ok(await this._dashboardRepository.GetAllSAPData());
        }
        #endregion


        [HttpGet]
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
