using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.DTOs;
using Books.Entities;
using Books.Helpers;

namespace Books.Interfaces
{
    public interface IReading
    {
         void AddRead(Read read);
        
        void DeleteRead(Read read);
        Task<Read> GetReads(int id);
        Task<PagedList<ReadingDto>> GetReadingForUser(ReadingHelpers readingHelpers);
        Task<IEnumerable<ReadingDto>> GetThread( string bookTitle);
        Task<bool> SaveAllAsync();
    }
}