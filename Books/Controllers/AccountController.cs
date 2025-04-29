using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Books.Data;
using Books.DTOs;
using Books.Entities;
using Books.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager,
         ITokenService tokenService, IMapper mapper
         )
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("register")] //POST : api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = this.mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result = await this.userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await this.userManager.AddToRoleAsync(user, "Member");

            if(!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token = await this.tokenService.CreateToken(user),
                KnownAs = user.KnownAs
            };
    
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await this.userManager.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username");

            var result = await this.userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!result) return Unauthorized("Invalid password");


           
            return new UserDto
            {
                Username = user.UserName,
                Token = await this.tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };

        }
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount()
        {
            // Get the current user
            var username = User.Identity?.Name;
            if (username == null) return Unauthorized();

            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found");

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest("Failed to delete user");

            return Ok(new { message = "Account deleted successfully" });
        }

        [Authorize]
        [HttpPost("change-password/{newPassword}")]
        public async Task<IActionResult> ChangePassword(string newPassword)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = await userManager.FindByNameAsync(username);

            if (user == null) return Unauthorized();

            var removePasswordResult = await userManager.RemovePasswordAsync(user);

            if (!removePasswordResult.Succeeded)
            {
                return BadRequest(removePasswordResult.Errors);
            }

            var addPasswordResult = await userManager.AddPasswordAsync(user, newPassword);

            if (!addPasswordResult.Succeeded)
            {
                return BadRequest(addPasswordResult.Errors);
            }

            return Ok(new { message = "Password changed successfully" });
        }

        private async Task<bool> UserExists(string username)
        {
            return await this.userManager.Users.AnyAsync(x => x.UserName == username.ToLower());//x refers to the Users method
        }
    }
}