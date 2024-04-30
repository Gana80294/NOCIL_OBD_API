using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Master
{
    public interface IRoleRepository : IRepository<Role>
    {
        bool AddRole(Role role);
        bool UpdateRole(Role role);
        bool DeleteRole(Role role);
        List<Role> GetRoles();
    }
}
