using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Master
{
    public interface IUserRepository:IRepository<User>
    {
        Task<bool> AddUser(UserDto user);
        Task<bool> SoftDeleteUser(UserDto userDto);
        Task<List<UserDto>> GetUsers();
        Task<bool> UpdateUser(UserDto userDto);
    }
}
