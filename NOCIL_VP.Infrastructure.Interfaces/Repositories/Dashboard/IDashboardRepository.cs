using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using NOCIL_VP.Domain.Core.Entities.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Dashboard
{
    public interface IDashboardRepository
    {
        Task<InitialData> GetInitialData(string employeeId);
        Task<List<DashboardDto>> GetInitiatedData(string employeeId);        
        Task<List<DashboardDto>> GetPendingData(string employeeId);
        Task<List<DashboardDto>> GetApprovedData(string employeeId);
        Task<List<DashboardDto>> GetRejectedData(string employeeId);

        Task<InitialData> GetAllData();
        Task<List<DashboardDto>> GetAllInitiatedData();
        Task<List<DashboardDto>> GetAllPendingData();
        Task<List<DashboardDto>> GetAllApprovedData();
        Task<List<DashboardDto>> GetAllRejectedData();
        Task<List<DashboardDto>> GetAllSAPData();
    }
}
