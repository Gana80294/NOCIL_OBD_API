using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Dashboard;

namespace NOCIL_VP.API.Controllers.Dashboard
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsRepository _notificationsRepository;
        public NotificationsController(INotificationsRepository notifications)
        {
            _notificationsRepository = notifications;
        }


        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllExpiryNotifications()
        {
            return Ok(await this._notificationsRepository.GetAllExpiryNotifications());
        }

        [HttpGet]
        [Authorize(Policy = "VendorPolicy")]
        public async Task<IActionResult> GetAllExpiryNotificationsByVendorCode(string vCode)
        {
            return Ok(await this._notificationsRepository.GetExpiryNotificationsByVendorCode(vCode));
        }
    }
}
