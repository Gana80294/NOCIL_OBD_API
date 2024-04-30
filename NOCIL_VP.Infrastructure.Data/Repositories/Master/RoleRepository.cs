using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Infrastructure.Interfaces.Repositories;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Master
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {

        private VpContext _dbContext;
        public RoleRepository(VpContext context) : base(context)
        {

        }

        public bool AddRole(Role role)
        {
            Add(role);
            var saved = Save();
            return saved;
        }

        public bool UpdateRole(Role role)
        {
            Update(role);
            var saved = Save();
            return saved;
        }

        public bool DeleteRole(Role role)
        {
            Remove(role);
            var saved = Save();
            return saved;
        }

        public List<Role> GetRoles()
        {
            return GetAll();
        }
    }
}
