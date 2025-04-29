using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.DTOs;
using Books.Entities;
using Books.Helpers;

namespace Books.Interfaces
{
    public interface ILikes
    {
        Task<FavoriteBook> GetBookLike(int sourceUserId, int TargetBookId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<IEnumerable<LikeDto>> GetLikesThread( string bookTitle, string username);
        Task<PagedList<LikeDto>> GetUserLikes(FollowsParams likesParams); 
    }
}