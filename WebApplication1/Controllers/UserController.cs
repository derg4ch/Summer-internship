using Logic.dto.users;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }


        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<UserInfoDto>> GetByUsername(string username)
        {
            UserInfoDto? user = await service.GetByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<UserInfoDto>> GetByEmail(string email)
        {
            var user = await service.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpGet("{id}/with-orders")]
        public async Task<ActionResult<UserInfoDto>> GetUserWithOrders(int id)
        {
            UserInfoDto? user = await service.GetUserWithOrdersAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        [HttpGet("with-orders")]
        public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetAllWithOrders()
        {
            var users = await service.GetAllWithOrdersAsync();
            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetAllUsers()
        {
            var users = await service.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoDto>> GetUserById(int id)
        {
            UserInfoDto? user = await service.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserInfoDto>> CreateUser([FromBody] UserNewDto newUser)
        {
            var createdUser = await service.CreateUserAsync(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfoDto>> UpdateUser(int id, [FromBody] UserEditDto userEdit)
        {
            UserInfoDto? updatedUser = await service.UpdateUserAsync(id, userEdit);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var deleted = await service.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
