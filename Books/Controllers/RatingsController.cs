using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.DTOs;
using Books.Entities;
using Books.Extensions;
using Books.Helpers;
using Books.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    public class RatingsController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IBooks _books;
        private readonly IMapper _mapper;
        private readonly IRatings _ratings;
        public RatingsController(IUserRepository userRepository, IRatings ratings,
        IBooks books, IMapper mapper)
        {
            _ratings = ratings;
            _mapper = mapper;
            _books = books;
            _userRepository = userRepository;
        }

        [HttpPost]
         public async Task<ActionResult<RatingDto>> CreateReview(CreateRatingDto createRatingDto)
        {
            var username = User.GetUsername();

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var book = await _books.GetBooksByTitleAsync(createRatingDto.BookTitle);
            if(book == null) return NotFound();

            var rating = new Rating 
            {
                Sender = sender,
                Book = book,
                SenderUsername = sender.UserName,
                BookTitle = book.Title,
                Rate = createRatingDto.Rate
            };
            _ratings.AddRate(rating);
            if(await _ratings.SaveAllAsync()) return Ok(_mapper.Map<RatingDto>(rating));
            return BadRequest("Failed to add rating");

        }

        [HttpGet]//toate review-urile
        public async Task<ActionResult<PagedList<RatingDto>>> GetRatingForUser([FromQuery] ReviewParams reviewParams)
        {
            reviewParams.Username = User.GetUsername();
            var rating = await _ratings.GetRatingsForUser(reviewParams);
            Response.AddPaginationHeader(new PaginationHeader(rating.CurrentPage,
            rating.PageSize, rating.TotalCount, rating.TotalPage));
            return rating;
        }

        [HttpGet("thread/{title}")]//review carte de la utilizator conectat
         public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewThread(string title)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _ratings.GetRatingThread(title));
        } 
    }
    }
