﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.users;

namespace Logic.Interfaces
{
    public interface IUserService
    {
        Task<UserInfoDto?> GetByUsernameAsync(string username);
        Task<UserInfoDto?> GetByEmailAsync(string email);
        Task<PagedList<UserInfoDto>> GetPagedUsersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<UserInfoDto>> GetAllUsersAsync();
        Task<UserInfoDto?> GetUserByIdAsync(int id);
        Task<UserInfoDto> CreateUserAsync(UserNewDto newUser);
        Task<UserInfoDto?> UpdateUserAsync(int id, UserEditDto userEdit, bool canChangeRole);
        Task<bool> DeleteUserAsync(int id);
    }
}
