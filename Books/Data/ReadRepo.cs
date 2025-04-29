using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.DTOs;
using Books.Entities;
using Books.Helpers;
using Books.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class ReadRepo : IRead
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ReadRepo(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            
        }
        public async Task<BooksRead> GetBooksRead(int sourceUserId, int TargetBookId)
        {
            return await _context.BooksReads.FindAsync(sourceUserId, TargetBookId);
        }

        public async Task<IEnumerable<ReadDto>> GetReadThread(string bookTitle)
        {
            var reads = await _context.BooksReads
            .Include(u => u.SourceUser).ThenInclude(p => p.Photos)
            .Include( u=> u.TargetBook).ThenInclude(p => p.BPhotos)
            .Where(m => m.TargetBook.Title ==bookTitle)
            .ToListAsync();

            return _mapper.Map<IEnumerable<ReadDto>>(reads);
        }

        public async Task<PagedList<ReadDto>> GetReadsForUser(FollowsParams readsParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var books = _context.BooksTable.OrderBy(u => u.Title).AsQueryable();
            var reads = _context.BooksReads.AsQueryable();

            if(readsParams.Predicate == "read")
            {
                reads = reads.Where(like => like.SourceUserId == readsParams.UserId);
                books = reads.Select(like => like.TargetBook);
            }

            var readsB =  books.Select(user => new ReadDto
            {
                Title = user.Title,
                Author = user.Author,
                PhotoUrl = user.BPhotos.FirstOrDefault(x => x.IsMain).UrlBook,
                Id = user.Id

            });
            return await PagedList<ReadDto>.CreateAsync(readsB, readsParams.PageNumber,
            readsParams.PageSize);
        }

        public async Task<AppUser> GetUserWithReads(int userId)
        {
            return await _context.Users
            .Include(x => x.ReadBooks)
            .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}