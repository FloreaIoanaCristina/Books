using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.DTOs;
using Books.Entities;
using Books.Helpers;
using Books.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class LikesRepository : ILikes
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public LikesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<FavoriteBook> GetBookLike(int sourceUserId, int TargetBookId)
        {
            return await _context.FavoriteBooks.FindAsync(sourceUserId, TargetBookId);
        }

        public async Task<IEnumerable<LikeDto>> GetLikesThread(string bookTitle, string username)
        {
            var likes = await _context.FavoriteBooks
            .Include(u => u.SourceUser).ThenInclude(p => p.Photos)
            .Include( u=> u.TargetBook).ThenInclude(p => p.BPhotos)
            .Where(m => m.TargetBook.Title ==bookTitle 
            &&  m.SourceUser.UserName == username)
            .ToListAsync();

            return _mapper.Map<IEnumerable<LikeDto>>(likes);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(FollowsParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var books = _context.BooksTable.OrderBy(u => u.Title).AsQueryable();
            var likes = _context.FavoriteBooks.AsQueryable();

            if(likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                books = likes.Select(like => like.TargetBook);
            }

            var likedB =  books.Select(user => new LikeDto
            {
                Title = user.Title,
                Author = user.Author,
                PhotoUrl = user.BPhotos.FirstOrDefault(x => x.IsMain).UrlBook,
                Id = user.Id

            });
            return await PagedList<LikeDto>.CreateAsync(likedB, likesParams.PageNumber,
            likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
            .Include(x => x.LikedBooks)
            .FirstOrDefaultAsync(x => x.Id == userId);
        }

        
    }
}