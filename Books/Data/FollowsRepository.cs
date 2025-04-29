using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.DTOs;
using Books.Entities;
using Books.Extensions;
using Books.Helpers;
using Books.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class FollowsRepository : IFollowers
    {
        private readonly DataContext _context;
        public FollowsRepository(DataContext context)
        {
            _context = context;
            
        }
        public async Task<UserFollow> GetUserFollow(int sourceUserId, int targetUserId)
        {
            return await _context.Followers.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<FollowDto>> GetUsersFollows(FollowsParams followsParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var follows = _context.Followers.AsQueryable();

            if(followsParams.Predicate == "followed")
            {
                follows = follows.Where(follow => follow.SourceUserId == followsParams.UserId);
                users = follows.Select(follow => follow.TargetUser);
            }

            if(followsParams.Predicate == "followedBy")
            {
                follows = follows.Where(follow => follow.TargetUserId == followsParams.UserId);
                users = follows.Select(follow => follow.SourceUser);
            }

            var followedUsers = users.Select(user => new FollowDto{
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id
            });
            return await PagedList<FollowDto>.CreateAsync(followedUsers, followsParams.PageNumber,
             followsParams.PageSize);
        }

        public async Task<AppUser> GetUserWithFollows(int userId)
        {
            return await _context.Users
            .Include(x => x.FollowedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);
        }

        
    }
}