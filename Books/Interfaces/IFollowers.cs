using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.DTOs;
using Books.Entities;
using Books.Helpers;

namespace Books.Interfaces
{
    public interface IFollowers
    {
        Task<UserFollow> GetUserFollow(int sourceUserId, int targetUserId);
        Task<AppUser> GetUserWithFollows(int userId);
        Task<PagedList<FollowDto>> GetUsersFollows(FollowsParams followsParams);
    }
}