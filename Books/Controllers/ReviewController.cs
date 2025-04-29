using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.DTOs;
using Books.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Books.Extensions;
using Books.Entities;
using Books.Helpers;

namespace Books.Controllers
{
    public class ReviewController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IReview _review;
        private readonly IBooks _books;
        private readonly IMapper _mapper;
        public ReviewController(IUserRepository userRepository, IReview review,
         IBooks books, IMapper mapper)
        {
            _mapper = mapper;
            _books = books;
            _review = review;
            _userRepository = userRepository;
        }


        [HttpPost]
        public async Task<ActionResult<ReviewDto>> CreateReview(CreateReviewDto createReviewDto)
        {
            var username = User.GetUsername();

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var book = await _books.GetBooksByTitleAsync(createReviewDto.BookTitle);
            if(book == null) return NotFound();

            var review = new Review 
            {
                Sender = sender,
                Book = book,
                SenderUsername = sender.UserName,
                BookTitle = book.Title,
                Content = createReviewDto.Content,
                Rate = createReviewDto.Rate
            };
            _review.AddReview(review);
            if(await _review.SaveAllAsync()) return Ok(_mapper.Map<ReviewDto>(review));
            return BadRequest("Failed to add review");

        }
        [HttpGet]//toate review-urile
        public async Task<ActionResult<PagedList<ReviewDto>>> GetReviewForUser([FromQuery] ReviewParams reviewParams)
        {
            reviewParams.Username = User.GetUsername();
            var review = await _review.GetReviewsForUser(reviewParams);
            Response.AddPaginationHeader(new PaginationHeader(review.CurrentPage,
            review.PageSize, review.TotalCount, review.TotalPage));
            return review;
        }

        [HttpGet("thread/{title}")]//review carte de la utilizator conectat
         public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewThread(string title)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _review.GetReviewThread(title));
        } 
    }
}