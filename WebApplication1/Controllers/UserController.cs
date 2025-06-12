using Logic;
using Logic.dto.users;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet("by-username/{username}")]
        [Authorize(Roles = "Manager")]
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
        [Authorize(Roles = "Manager")]
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
        [Authorize(Roles = "Manager")]
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

        [HttpGet("paginated")]
        [Authorize(Roles = "Manager")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoDto>> GetUserById(int id)
        {
            try
            {
                if (!IsOwnerOrManager(id))
                {
                    return Forbid();
                }

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
        [Authorize(Roles = "Manager")]
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
                if (!IsOwnerOrManager(id))
                {
                    return Forbid();
                }

                if (!string.IsNullOrEmpty(userEdit.Role) && !User.IsInRole("Manager"))
                {
                    return Forbid("Only managers can change user roles.");
                }

                UserInfoDto? updatedUser = await service.UpdateUserAsync(id, userEdit, User.IsInRole("Manager"));
                
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
        [Authorize(Roles = "Manager")]
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

        private bool IsOwnerOrManager(int userId)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (currentUserIdClaim == null)
            {
                return false;
            }

            bool isManager = User.IsInRole("Manager");
            if (isManager || currentUserIdClaim == userId.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}