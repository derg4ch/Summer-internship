using Logic;
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
            try
            {
                UserInfoDto? user = await service.GetByUsernameAsync(username);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<UserInfoDto>> GetByEmail(string email)
        {
            try
            {
                var user = await service.GetByEmailAsync(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetAllUsers()
        {
            try
            {
                var users = await service.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoDto>> GetUserById(int id)
        {
            try
            {
                UserInfoDto? user = await service.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserInfoDto>> CreateUser([FromBody] UserNewDto newUser)
        {
            try
            {
                var createdUser = await service.CreateUserAsync(newUser);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfoDto>> UpdateUser(int id, [FromBody] UserEditDto userEdit)
        {
            try
            {
                UserInfoDto? updatedUser = await service.UpdateUserAsync(id, userEdit);
                if (updatedUser == null)
                {
                    return NotFound();
                }
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var deleted = await service.DeleteUserAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedList<UserInfoDto>>> GetPagedUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pagedUsers = await service.GetPagedUsersAsync(pageNumber, pageSize);
                return Ok(pagedUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}