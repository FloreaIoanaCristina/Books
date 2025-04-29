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
    public class FavoriteController : BaseApiController
    {
        private readonly ILikes _like;
        private readonly IUserRepository _userRepository;
        private readonly IBooks _books;
        public FavoriteController(ILikes like, IUserRepository userRepository,
        IBooks books)
        {
            _books = books;
            _userRepository = userRepository;
            _like = like;
            
        }

        [HttpPost("{title}")]
        public async Task<ActionResult> AddLike (string title)
        { 
            var sourceUserId = User.GetUserId();
            var bookLiked = await _books.GetBooksByTitleAsync(title);
            var sourceUser = await _like.GetUserWithLikes(sourceUserId);

            if(bookLiked ==  null) return NotFound();

            var bookLike = await _like.GetBookLike(sourceUserId, bookLiked.Id);

            if(bookLike != null) return BadRequest("Already liked");

            bookLike = new FavoriteBook
            {
                SourceUserId = sourceUserId,
                TargetBookId = bookLiked.Id
            };

            sourceUser.LikedBooks.Add(bookLike);
            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like");
            
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] FollowsParams likeParams)
        {
            likeParams.UserId = User.GetUserId();
            var users = await _like.GetUserLikes(likeParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage,users.PageSize,
            users.TotalCount,users.TotalPage));

            return Ok(users);

        }
        [HttpGet("thread/{username}")]//review carte de la utilizator conectat
         public async Task<ActionResult<IEnumerable<LikeDto>>> GetLikeThread(string title, string username)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _like.GetLikesThread(title, username));
        } 

        
    }
}