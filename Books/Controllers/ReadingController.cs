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
    public class ReadingController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IBooks _books;
        private readonly IReading _reading;
        public ReadingController(IUserRepository userRepository, IMapper mapper,
        IBooks books, IReading reading)
        {
            _reading = reading;
            _books = books;
            _mapper = mapper;
            _userRepository = userRepository;
            
        }

         [HttpPost]
        public async Task<ActionResult<ReadingDto>> CreateReview(CreateReadDto createReadDto)
        {
            var username = User.GetUsername();

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var book = await _books.GetBooksByTitleAsync(createReadDto.BookTitle);
            if(book == null) return NotFound();

            var read = new Read 
            {
                Sender = sender,
                Book = book,
                SenderUsername = sender.UserName,
                BookTitle = book.Title,
                
            };
            _reading.AddRead(read);
            if(await _reading.SaveAllAsync()) return Ok(_mapper.Map<ReadingDto>(read));
            return BadRequest("Failed to add review");

        }

        [HttpGet]//toate review-urile
        public async Task<ActionResult<PagedList<ReadingDto>>> GetReadingForUser([FromQuery] ReadingHelpers readingHelpers)
        {
            readingHelpers.Username = User.GetUsername();
            var read = await _reading.GetReadingForUser(readingHelpers);
            Response.AddPaginationHeader(new PaginationHeader(read.CurrentPage,
            read.PageSize, read.TotalCount, read.TotalPage));
            return read;
        }

        [HttpGet("thread/{member}")]//review carte de la utilizator conectat
         public async Task<ActionResult<IEnumerable<ReadingDto>>> GetThread(string member)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _reading.GetThread(member));
        } 
    }
}