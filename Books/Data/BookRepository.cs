using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Books.DTOs;
using Books.Entities;
using Books.Helpers;
using Books.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class BookRepository : IBooks
    {
        private readonly DataContext _context;
   
        private readonly IMapper _mapper;

        public BookRepository(DataContext context, IMapper mapper) 
        
        {
            _mapper = mapper;
           
            _context = context;
        }

        public void AddProperty(AppBooks books)
        {
            _context.BooksTable.Add(books);
        }

        public void DeleteProperty(int id)
        {
            throw new NotImplementedException();
        }

        // public async Task<AppBooks> GetBookByIdAsync(int id)
        // {
        //     return await _context.BooksTable
        //     .Include(p => p.BPhotos)
        //     .FirstOrDefaultAsync(p => p.Id == id);
        // }

        // public async Task<AppBooks> GetBookByTitleAsync(string title)
        // {
        //     return await _context.BooksTable
        //     .Include(p => p.BPhotos).SingleOrDefaultAsync(x => x.Title == title);
        // }

        public async Task<AppBooks> GetBookDetailAsync(int id) 
        {
             var books = await _context.BooksTable
            .Include(p => p.Genre)
            .Include(p => p.BPhotos)
            .Where(p => p.Id == id)
            .FirstAsync();

            return books;
        }

        public async Task<PagedList<BookDto>> GetBooksAsync(BookParams bookParams, int? genreId) 
        {
            var query = _context.BooksTable.OrderBy(u => u.Title)
            
            .Where(x=> x.GenreId == genreId).AsQueryable();
            
            var books = query.Select( book => new BookDto 
            {
                Title = book.Title,
                Author = book.Author,
                PhotoUrl = book.BPhotos.FirstOrDefault(x => x.IsMain).UrlBook,
                Id = book.Id,
                
                
               
           


            });

            return await PagedList<BookDto>.CreateAsync(books, bookParams.PageNumber, bookParams.PageSize);


        }


        public async Task<AppBooks> GetBooksByIdAsync(int id)
        {
             var books = await _context.BooksTable
            .Include(p => p.BPhotos)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

            return books;
        }
        public async Task<AppBooks> GetBooksByTitleAsync(string title)
        {
            var books = await _context.BooksTable
            .Include(p => p.BPhotos)
            .Where(p => p.Title == title)
            .FirstOrDefaultAsync();

            return books;
        }

         public async Task<AppBooks> FindBook(int id)
        {
            return await _context.BooksTable.FindAsync(id);
        }

        // public async Task<IEnumerable<Genre>> GetGenreAsync()
        // {
        //    return await _context.Genres.ToListAsync();
        // }

        public async Task<BookDto> GetTitleAsync(string title)
        {
            return await _context.BooksTable
            .Where(x => x.Title == title)
            .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        // public async Task<IEnumerable<BookDto>> GetTitlesAsync()
        // {
        //     return await _context.BooksTable.ProjectTo<BookDto>(_mapper.ConfigurationProvider)
        //     .ToListAsync();
        // }

        // public async Task<bool> SaveAllAsync()
        // {
        //     return await _context.SaveChangesAsync() > 0;
        // }

        public void UpdateBooks(AppBooks books)
        {
            _context.Entry(books).State = EntityState.Modified;
        }

       
    }
}