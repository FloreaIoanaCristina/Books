using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.Data;
using Books.DTOs;
using Books.Entities;
using Books.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Books.Controllers
{
    public class GenreController : BaseApiController
    {
     
        private readonly IMapper _mapper;
  
        private readonly IUnitOfWork _unitOfWork;
        public GenreController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }


        [HttpGet("genres")]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _unitOfWork.GenreRepo.GetGenresAsync();
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            return Ok(genres);
        }

         [HttpPost("post")] 
        public async Task<IActionResult> AddGenre(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            _unitOfWork.GenreRepo.AddGenre(genre);
            await _unitOfWork.SaveAsync();
            return StatusCode(201);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            _unitOfWork.GenreRepo.DeleteGenre(id);
            await _unitOfWork.SaveAsync();
            return Ok(id);
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateGenre(int id, GenreDto genreDto)
        {
            var genreDb = await _unitOfWork.GenreRepo.FindGenre(id);

             _mapper.Map(genreDto, genreDb);
        
            await _unitOfWork.SaveAsync();
            return StatusCode(200);
        }



        




       

    }
}