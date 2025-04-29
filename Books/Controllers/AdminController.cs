using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Books.Interfaces;
using Books.DTOs;

namespace Books.Controllers
{
    public class AdminController : BaseApiController {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoRepository photoRepository;
        private readonly IUserRepository userRepository;
        private readonly IPhotoService photoService;
        public AdminController(UserManager<AppUser> userManager, IPhotoRepository photoRepository,IPhotoService photoService, IUserRepository userRepository)
        {
            _userManager = userManager;
            this.photoRepository = photoRepository;
            this.photoService = photoService;
            this.userRepository = userRepository;
        }

        [Authorize(Policy ="RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
            .OrderBy(u => u.UserName)
            .Select(u => new
            {
                u.Id,
                Username = u.UserName,
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
            })
            .ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery]string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must at least have one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if(user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if(!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if(!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

       [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {
            var photos = await this.photoRepository.GetUnapprovedPhotos();
            return Ok(photos);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<ActionResult> ApprovePhoto(int photoId)
        {
            var photo = await this.photoRepository.GetPhotoById(photoId);
            photo.IsApproved = true;

            var user = await this.userRepository.GetUserByPhotoId(photoId);
            if(!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

            await this.photoRepository.Complete();

            return Ok();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<ActionResult> RejectPhoto(int photoId)
        {
            var photo = await this.photoRepository.GetPhotoById(photoId);
            if(photo.PublicId != null)
            {
                var result = await this.photoService.DeletePhotoAsync(photo.PublicId);

                if( result.Result == "ok")
                {
                    this.photoRepository.RemovePhoto(photo);
                }
            }
            else
            {
                this.photoRepository.RemovePhoto(photo);

                
            }
            await this.photoRepository.Complete();

            return Ok();

        }
    }
}