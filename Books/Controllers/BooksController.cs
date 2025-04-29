using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.Data;
using Books.DTOs;
using Books.Entities;
using Books.Extensions;
using Books.Helpers;
using Books.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
 //   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BooksController : ControllerBase
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;


        public BooksController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
           
            _photoService = photoService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
           
        }
        [Authorize]
        [HttpGet("detail/{title}")]
       
        public async Task<ActionResult<BookDto>> GetBookDetail(string title)
        {
            return await _unitOfWork.BookRepository.GetTitleAsync(title);
            
        }
        //[Authorize]
         [HttpPost("add")]
        public async Task<IActionResult> AddBook(AddBookDto addBookDto)
        {
            var book = _mapper.Map<AppBooks>(addBookDto);
            _unitOfWork.BookRepository.AddProperty(book);
            await _unitOfWork.SaveAsync();
            return StatusCode(201);
        }
        [HttpPost("add/photo/{bookId}")]
        //[Authorize]
        public async Task<ActionResult<BookPhotoDto>> AddPhoto(IFormFile file, int bookId)
        {
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null)
                return BadRequest(result.Error.Message);
          

            var property = await _unitOfWork.BookRepository.GetBooksByIdAsync(bookId);

            // if (property.PostedBy != userId)
            //     return BadRequest("You are not authorised to upload photo for this property");

            var photo = new BookPhoto
            {
                UrlBook = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (property.BPhotos.Count == 0)
            {
                photo.IsMain = true;
            }

            property.BPhotos.Add(photo);
            if (await _unitOfWork.SaveAsync()) return _mapper.Map<BookPhotoDto>(photo);

            return BadRequest("Some problem occured in uploading photo..");
        }
        [Authorize]
         [HttpPost("set-main-photo/{photoId}/{id}")]
        public async Task<IActionResult> SetMainPhoto(int photoId, int id)
        {
           var book = await _unitOfWork.BookRepository.GetBooksByIdAsync(photoId);

            var photo = book.BPhotos.FirstOrDefault(p => p.Id == id);

            if (photo == null)
                return BadRequest("No such property or photo exists");

            if (photo.IsMain)
                return BadRequest("This is already a primary photo");


            var currentPrimary = book.BPhotos.FirstOrDefault(p => p.IsMain);
            if (currentPrimary != null) currentPrimary.IsMain = false;
            photo.IsMain = true;

            if (await _unitOfWork.SaveAsync()) return NoContent();

            return BadRequest("Failed to set primary photo");
            
        }
        [Authorize]
        [HttpDelete("delete-photo/{photoId}/{id}")]
        public async Task<ActionResult> DeletePhoto(int photoId, int id)
        {
            var book = await _unitOfWork.BookRepository.GetBooksByIdAsync(photoId);

            var photo = book.BPhotos.FirstOrDefault(x => x.Id == id);

            if(photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("you cannot delete your main photo");

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);

            }
            book.BPhotos.Remove(photo);

            if (await _unitOfWork.SaveAsync()) return Ok();
            return BadRequest("Problem deleting photo");
        }
        

        [HttpGet]
        public async Task<ActionResult<PagedList<BookDto>>> GetBooks([FromQuery] BookParams bookParams, int? genreId)
        {
            var books = await _unitOfWork.BookRepository.GetBooksAsync(bookParams,genreId);
            Response.AddPaginationHeader(new PaginationHeader(books.CurrentPage,books.PageSize,books.TotalCount,
            books.TotalPage));
          
            return Ok(books);
        }

      





        
        
    }
}