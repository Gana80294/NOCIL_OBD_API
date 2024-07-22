using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NOCIL_VP.Domain.Core.Configurations;
using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Mappings;
using NOCIL_VP.Domain.Core.Entities.Master;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Master
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly VpContext _dbContext;
        private readonly AppSetting _appSettings;

        public UserRepository(VpContext vpContext,IOptions<AppSetting> options) : base(vpContext)
        {
            _dbContext = vpContext;
            _appSettings = options.Value;
        }

        public async Task<bool> AddUser(UserDto userDto)
        {
            User user = new User
            {
                Employee_Id = userDto.Employee_Id,
                Password = PasswordEncryptor.EncryptPassword(_appSettings.DefaultPassword),
                Email = userDto.Email,
                Mobile_No = userDto.Mobile_No,
                First_Name = userDto.First_Name,
                Middle_Name = userDto.Middle_Name,
                Last_Name = userDto.Last_Name,
                Is_Active = true,
                Reporting_Manager_EmpId = userDto.Reporting_Manager_EmpId
            };
            Add(user);
            User_Role_Mapping user_Role_Mapping = new User_Role_Mapping();
            user_Role_Mapping.Employee_Id = userDto.Employee_Id;
            user_Role_Mapping.Role_Id = userDto.Role_Id;
            _dbContext.User_Role_Mappings.Add(user_Role_Mapping);
            var saved = await SaveAsync();
            return saved;
        }

        public async Task<bool> SoftDeleteUser(UserDto userDto)
        {
            var user = _dbContext.Users.Where(x => x.Employee_Id == userDto.Employee_Id).FirstOrDefault();
            user.Is_Active = false;
            var softDeleted = Save();
            return softDeleted;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users = (from user in _dbContext.Users
                         join userRole in _dbContext.User_Role_Mappings on user.Employee_Id equals userRole.Employee_Id
                         select new UserDto
                         {
                             Employee_Id = user.Employee_Id,
                             Role_Id = userRole.Role_Id,
                             First_Name = user.First_Name,
                             Middle_Name = user.Middle_Name,
                             Last_Name = user.Last_Name,
                             Email = user.Email,
                             Mobile_No = user.Mobile_No,
                             Reporting_Manager_EmpId = user.Reporting_Manager_EmpId,
                             IsActive = user.Is_Active,
                             Display_Name = $"{user.First_Name} {user.Middle_Name} {user.Last_Name}".TrimEnd()
                         }).ToList();
            return users;
        }

        public async Task<bool> UpdateUser(UserDto userDto)
        {
            var existingUser = _dbContext.Users.Where(x => x.Employee_Id == userDto.Employee_Id).FirstOrDefault();
            existingUser.First_Name = userDto.First_Name;
            existingUser.Middle_Name = userDto.Middle_Name;
            existingUser.Last_Name = userDto.Last_Name;
            existingUser.Email = userDto.Email;
            existingUser.Mobile_No = userDto.Mobile_No;
            existingUser.Reporting_Manager_EmpId = userDto.Reporting_Manager_EmpId;
            existingUser.Is_Active = userDto.IsActive;

            var userRole = _dbContext.User_Role_Mappings.Where(x => x.Employee_Id == userDto.Employee_Id).FirstOrDefault();
            userRole.Role_Id = userDto.Role_Id;
            var updated = Save();
            return updated;
        }

    }
}
