using NOCIL_VP.Domain.Core.Dtos.Registration;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration
{
    public interface ITransportRepository
    {
        public Task<bool> SaveTransportVendorDetails(TransportForm transportForm, int formId);
        public Task<bool> UpdateTransportVendorDetails(TransportForm transportForm, int formId);
    }
}
