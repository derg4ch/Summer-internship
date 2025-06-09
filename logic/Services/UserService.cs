using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.dto.users;
using Logic.Interfaces;
using Work_with_db.Repo;
using Work_with_db.Tables;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private IUserRepository repo;

        public UserService(IUserRepository userRepository)
        {
            this.repo = userRepository;
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
                Username = user.Username,
                Email = user.Email,
                OrdersCount = ordersCount
            };
        }

        public async Task<UserInfoDto?> GetByEmailAsync(string email)
        {
            User? user = await repo.GetByEmailAsync(email);
            if (user == null) return null;

            int ordersCount = await repo.GetOrdersCountByUserIdAsync(user.Id);

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                OrdersCount = ordersCount
            };
        }

        public async Task<UserInfoDto?> GetUserWithOrdersAsync(int userId)
        {
            User? user = await repo.GetUserWithOrdersAsync(userId);
            if (user == null) return null;

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                OrdersCount = user.Orders?.Count ?? 0
            };
        }

        public async Task<IEnumerable<UserInfoDto>> GetAllWithOrdersAsync()
        {
            var users = await repo.GetAllWithOrdersAsync();

            return users.Select(newUser => new UserInfoDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                OrdersCount = newUser.Orders?.Count ?? 0
            });
        }

        public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync()
        {
            var users = await repo.GetAllAsync();
            List<UserInfoDto> userDtos = new List<UserInfoDto>();
            
            foreach (User user in users)
            {
                int count = await repo.GetOrdersCountByUserIdAsync(user.Id);
                userDtos.Add(new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    OrdersCount = count
                });
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

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                OrdersCount = count
            };
        }

        public async Task<UserInfoDto> CreateUserAsync(UserNewDto newUser)
        {
            User user = new User
            {
                Username = newUser.Username,
                Email = newUser.Email,
                Password = newUser.Password 
            };

            await repo.AddAsync(user);

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                OrdersCount = 0
            };
        }

        public async Task<UserInfoDto?> UpdateUserAsync(int id, UserEditDto userEdit)
        {
            User? user = await repo.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            
            user.Username = userEdit.Username;
            user.Email = userEdit.Email;
            user.Password = userEdit.Password; 

            await repo.UpdateAsync(user);

            int count = await repo.GetOrdersCountByUserIdAsync(user.Id);

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                OrdersCount = count
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            User? user = await repo.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            await repo.DeleteAsync(user);
            return true;
        }
    }
}
