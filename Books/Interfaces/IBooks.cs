using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.DTOs;
using Books.Entities;
using Books.Helpers;

namespace Books.Interfaces
{
    public interface IBooks
    {
        Task<PagedList<BookDto>> GetBooksAsync(BookParams bookParams, int? genreId);
        Task<AppBooks> GetBookDetailAsync(int id);
        Task<AppBooks> GetBooksByIdAsync(int id);
        Task<AppBooks> GetBooksByTitleAsync(string title);
        void AddProperty(AppBooks books);

        void DeleteProperty(int id);
         public Task<AppBooks> FindBook(int id);

         Task<BookDto> GetTitleAsync(string title);


    }
}