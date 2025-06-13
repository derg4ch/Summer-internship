using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Work_with_db.Tables;

namespace Logic.services
{
    public class AuthService
    {
        private UserManager<User> userManage;
        private RoleManager<IdentityRole<int>> roles;
        private IConfiguration config;
        private ClothingStoreDbContext database;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration, ClothingStoreDbContext dbContext)
        {
            userManage = userManager;
            roles = roleManager;
            config = configuration;
            database = dbContext;
        }

        public async Task<IdentityResult> RegisterAsync(string username, string email, string password)
        {
            User user = new User { 
                UserName = username, 
                Email = email 
            };

            var result = await userManage.CreateAsync(user, password);
            
            if (!result.Succeeded)
            {
                return result;
            }
            if (!await roles.RoleExistsAsync("Customer"))
            {
                await roles.CreateAsync(new IdentityRole<int>("Customer"));
            }

            await userManage.AddToRoleAsync(user, "Customer");
            return result;
        }

        public async Task<(string AccessToken, string RefreshToken)> LoginAsync(string username, string password)
        {
            User user = await userManage.FindByNameAsync(username);
            if (user == null || !await userManage.CheckPasswordAsync(user, password))
            {
                return (null, null);
            }

            string accessToken = await GenerateJwtToken(user);
            string refreshToken = GenerateRefreshToken();

            RefreshToken refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(30).ToUniversalTime(),
                UserId = user.Id
            };

            database.RefreshTokens.Add(refreshTokenEntity);
            await database.SaveChangesAsync();

            return (accessToken, refreshToken);
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(int userId, string refreshToken)
        {
            var savedRefreshToken = await database.RefreshTokens.Where(rt => rt.UserId == userId && rt.Token == refreshToken).FirstOrDefaultAsync();

            if (savedRefreshToken == null || savedRefreshToken.ExpiryDate <= DateTime.UtcNow.ToUniversalTime())
            {
                return (null, null);
            }

            var user = await userManage.FindByIdAsync(userId.ToString());
            if (user == null)
                return (null, null);

            string newAccessToken = await GenerateJwtToken(user);
            string newRefreshToken = GenerateRefreshToken();

            database.RefreshTokens.Remove(savedRefreshToken);

            RefreshToken newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(30).ToUniversalTime(),
                UserId = user.Id
            };

            database.RefreshTokens.Add(newRefreshTokenEntity);
            await database.SaveChangesAsync();

            return (newAccessToken, newRefreshToken);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var userRoles = await userManage.GetRolesAsync(user);
            List<Claim> authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string refreshToken)
        {
            return await database.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}