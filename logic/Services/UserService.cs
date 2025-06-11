using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.users;
using Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Work_with_db.Repo;
using Work_with_db.Tables;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repo;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly ClothingStoreDbContext context;
        private readonly PasswordHasher<User> passwordHasher;

        public UserService(IUserRepository userRepository, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, ClothingStoreDbContext context)
        {
            this.repo = userRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
            this.passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserInfoDto?> GetByUsernameAsync(string username)
        {
            User? user = await repo.GetByUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            int ordersCount = await repo.GetOrdersCountByUserIdAsync(user.Id);

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                OrdersCount = ordersCount
            };
        }

        public async Task<UserInfoDto?> GetByEmailAsync(string email)
        {
            User? user = await repo.GetByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            int ordersCount = await repo.GetOrdersCountByUserIdAsync(user.Id);

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                OrdersCount = ordersCount
            };
        }

        public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync()
        {
            var allUsers = await repo.GetAllAsync();
            List<UserInfoDto> userDtos = new List<UserInfoDto>();

            foreach (User user in allUsers)
            {
                int count = await repo.GetOrdersCountByUserIdAsync(user.Id);
                UserInfoDto userInfoDto = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    OrdersCount = count
                };

                userDtos.Add(userInfoDto);
            }
            return userDtos;
        }

        public async Task<UserInfoDto?> GetUserByIdAsync(int id)
        {
            var user = await repo.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            int count = await repo.GetOrdersCountByUserIdAsync(user.Id);
            UserInfoDto userInfoDto = new UserInfoDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                OrdersCount = count
            };

            return userInfoDto;
        }

        public async Task<UserInfoDto> CreateUserAsync(UserNewDto newUser)
        {
            if (await userManager.Users.AnyAsync(u => u.Email == newUser.Email))
            {
                throw new Exception("User with this email already exists");
            }

            if (await userManager.Users.AnyAsync(u => u.UserName == newUser.Username))
            {
                throw new Exception("User with this username already exists");
            }

            User user = new User
            {
                UserName = newUser.Username,
                Email = newUser.Email,
                NormalizedUserName = newUser.Username.ToUpper(),
                NormalizedEmail = newUser.Email.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, newUser.Password);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user");
            }

            var customerRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Manager");
            
            if (customerRole != null)
            {
                await userManager.AddToRoleAsync(user, "Manager");
            }

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                OrdersCount = 0
            };
        }

        public async Task<UserInfoDto?> UpdateUserAsync(int id, UserEditDto userEdit, bool canChangeRole)
        {
            User user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return null;
            }

            if (user.Email != userEdit.Email && await userManager.Users.AnyAsync(u => u.Email == userEdit.Email && u.Id != id))
            {
                throw new Exception("User with this email already exists");
            }

            if (user.UserName != userEdit.Username && await userManager.Users.AnyAsync(u => u.UserName == userEdit.Username && u.Id != id))
            {
                throw new Exception("User with this username already exists");
            }

            user.UserName = userEdit.Username;
            user.Email = userEdit.Email;
            user.NormalizedUserName = userEdit.Username.ToUpper();
            user.NormalizedEmail = userEdit.Email.ToUpper();

            if (!string.IsNullOrEmpty(userEdit.Password))
            {
                user.PasswordHash = passwordHasher.HashPassword(user, userEdit.Password);
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to update user");
            }

            // Логіка зміни ролі тільки якщо canChangeRole == true
            if (canChangeRole && !string.IsNullOrEmpty(userEdit.Role))
            {
                var currentRoles = await userManager.GetRolesAsync(user);
                var roleToSet = userEdit.Role;

                // Зняти всі ролі (або лише першу, якщо у вас одна роль на користувача)
                var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    throw new Exception("Failed to remove current roles");
                }

                var addRoleResult = await userManager.AddToRoleAsync(user, roleToSet);
                if (!addRoleResult.Succeeded)
                {
                    throw new Exception($"Failed to add role {roleToSet}");
                }
            }

            int count = await repo.GetOrdersCountByUserIdAsync(user.Id);

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                OrdersCount = count
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            User user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return false;
            }

            var result = await userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<PagedList<UserInfoDto>> GetPagedUsersAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            var users = await repo.GetPagedUsersAsync(skip, pageSize);
            int totalCount = await repo.getAllCounts();

            List<UserInfoDto> userDtos = new List<UserInfoDto>();
            foreach (var user in users)
            {
                int ordersCount = await repo.GetOrdersCountByUserIdAsync(user.Id);
                userDtos.Add(new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    OrdersCount = ordersCount
                });
            }

            var pagedList = new PagedList<UserInfoDto>(userDtos, pageNumber, pageSize, totalCount);
            return pagedList;
        }
    }
}