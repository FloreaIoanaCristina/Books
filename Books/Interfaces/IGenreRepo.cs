using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Entities;

namespace Books.Interfaces
{
    public interface IGenreRepo
    {
         Task<IEnumerable<Genre>> GetGenresAsync();
         void AddGenre(Genre genre);
         void DeleteGenre(int genreId);
         Task<Genre> FindGenre(int id);
       
    }
}