﻿using System.Security.Claims;
using Logic.dto.authorization;
using Logic.dto.users;
using Logic.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService service;

        public AuthController(AuthService authService)
        {
            service = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserNewDto request)
        {
            var result = await service.RegisterAsync(request.Username, request.Email, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var tokens = await service.LoginAsync(request.Username, request.Password);
            if (tokens.AccessToken == null)
            {
                return Unauthorized();
            }

            return Ok(new { accessToken = tokens.AccessToken, refreshToken = tokens.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto request)
        {
            var savedRefreshToken = await service.GetRefreshTokenAsync(request.RefreshToken);

            if (savedRefreshToken == null || savedRefreshToken.ExpiryDate <= DateTime.UtcNow)
            {
                return Unauthorized();
            }

            var tokens = await service.RefreshTokensAsync(savedRefreshToken.UserId, request.RefreshToken);

            if (tokens.AccessToken == null)
            {
                return Unauthorized();
            }

            return Ok(new { accessToken = tokens.AccessToken, refreshToken = tokens.RefreshToken });
        }
    }
}
