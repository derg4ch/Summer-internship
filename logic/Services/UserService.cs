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

        public async Task<UserInfoDto?> GetUserWithOrdersAsync(int userId)
        {
            User? user = await repo.GetUserWithOrdersAsync(userId);
            if (user == null)
            {
                return null;
            }
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
            var allUsers = await repo.GetAllWithOrdersAsync();

            return allUsers.Select(p => new UserInfoDto
            {
                Id = p.Id,
                Username = p.Username,
                Email = p.Email,
                OrdersCount = p.Orders?.Count ?? 0
            });
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
                    Username = user.Username,
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
                Username = user.Username,
                Email = user.Email,
                OrdersCount = count
            };

            return userInfoDto;
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
           
            UserInfoDto userInfoDto = new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                OrdersCount = 0
            };

            return userInfoDto;
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
            
            UserInfoDto userInfoDto = new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                OrdersCount = count
            };
            return userInfoDto;
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

        public async Task<PagedList<UserInfoDto>> GetPagedUsersAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            var users = await repo.GetPagedUsersAsync(skip, pageSize);
            int totalCount = await repo.getAllCounts();

            var userDtos = users.Select(u => new UserInfoDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                OrdersCount=users.Count
            }).ToList();

            var pagedList = new PagedList<UserInfoDto>(userDtos, pageNumber, pageSize, totalCount);
            return pagedList;
        }
    }
}
