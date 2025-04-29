using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Entities;
using Books.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class GenreRepo : IGenreRepo
    {
        private readonly DataContext _dataBooks;
        public GenreRepo(DataContext dataBooks)
        {
            _dataBooks = dataBooks;
            
        }
        public void AddGenre(Genre genre)
        {
            _dataBooks.Genres.AddAsync(genre);
        }

        public void DeleteGenre(int genreId)
        {
            var genre = _dataBooks.Genres.Find(genreId);
            _dataBooks.Genres.Remove(genre);
        }

        public async Task<Genre> FindGenre(int id)
        {
            return await _dataBooks.Genres.FindAsync(id);
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await _dataBooks.Genres.ToListAsync();
        }

       
    }
}