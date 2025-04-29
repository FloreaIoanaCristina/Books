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
    public class ReadingRepository : IReading
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public ReadingRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public void AddRead(Read read)
        {
            _context.Reads.Add(read);
        }

        public void DeleteRead(Read read)
        {
            _context.Reads.Remove(read);
        }

        public async Task<PagedList<ReadingDto>> GetReadingForUser(ReadingHelpers readingHelpers)
        {
             var query = _context.Reads
            .OrderBy( m => m.ReadSent)
            .AsQueryable();

            // query = reviewParams.Container switch 
            // {
            //     "Inbox" => query.Where(u => u.BookTitle == reviewParams.BookTitle) ,
            //     _ => query.Where(u => u.BookTitle == reviewParams.BookTitle && u.DateSent == null)
            // };

            var readings = query.ProjectTo<ReadingDto>(_mapper.ConfigurationProvider);
            return await PagedList<ReadingDto>.CreateAsync(readings, readingHelpers.PageNumber, readingHelpers.PageSize);
        }

        public async Task<IEnumerable<ReadingDto>> GetThread(string bookTitle)
        {
            var readings = await _context.Reads
           .Include(u => u.Sender).ThenInclude( p => p.Photos)
           .Include(u => u.Book).ThenInclude(p => p.BPhotos)
           .Where(
            m => m.Sender.UserName == bookTitle 
        //    && m.Sender.UserName == currentUsername ||
        //     m.Book.Title == currentUsername && m.Sender.UserName == bookTitle
            
           
           )
           .OrderByDescending(m => m.ReadSent)
           .ToListAsync();

           //var unreadMessages = messages.Where(m => m.DateRead == null && m.Recipient.UserName == currentUsername).ToList(); 
          
        //    if(unreadMessages.Any())
        //    {
        //        foreach (var message in unreadMessages)
        //        {
        //            message.DateRead = DateTime.UtcNow;
        //        }
        //        await  _context.SaveChangesAsync();
        //    }
           return _mapper.Map<IEnumerable<ReadingDto>>(readings); 
        }

        public async Task<Read> GetReads(int id)
        {
            return await _context.Reads
            .Include(u => u.Sender)
            .Include(u => u.Book)
            .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() >0;
        }
    }
}