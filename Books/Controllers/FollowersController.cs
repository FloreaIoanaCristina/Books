using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.DTOs;
using Books.Entities;
using Books.Extensions;
using Books.Helpers;
using Books.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    public class FollowersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IFollowers _followers;

        public FollowersController(IUserRepository userRepository, IFollowers followers)
        {
            _userRepository = userRepository;
            _followers = followers;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddFollow( string username)
        {
            var sourceUserId = User.GetUserId();
            var followedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _followers.GetUserWithFollows(sourceUserId);

            if(followedUser == null) return NotFound();

            if(sourceUser.UserName == username) return BadRequest("You can't follow yourself");
           
            var userFollow = await _followers.GetUserFollow(sourceUserId, followedUser.Id);

            if(userFollow != null) return BadRequest("already followed");

            userFollow = new UserFollow
            {
                SourceUserId = sourceUserId,
                TargetUserId = followedUser.Id
            };

            sourceUser.FollowedUsers.Add(userFollow);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to follow");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<FollowDto>>> GetUserFollowers([FromQuery]FollowsParams followsParams)
        {

            followsParams.UserId = User.GetUserId();
            var users = await _followers.GetUsersFollows(followsParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,
             users.PageSize,
             users.TotalCount, users.TotalPage));
            return Ok(users);
        }

    }
}