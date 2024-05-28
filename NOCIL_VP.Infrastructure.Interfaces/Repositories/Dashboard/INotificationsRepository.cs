using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Dashboard
{
    public interface INotificationsRepository
    {
        Task<List<ExpiryNotificationDto>> GetAllExpiryNotifications();
        Task<List<ExpiryNotificationDto>> GetExpiryNotificationsByVendorCode(string vCode);
    }
}
