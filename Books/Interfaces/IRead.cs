using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Entities;
using Books.DTOs;
using Books.Helpers;

namespace Books.Interfaces
{
    public interface IRead
    {
        Task<BooksRead> GetBooksRead(int sourceUserId, int TargetBookId);
        Task<AppUser> GetUserWithReads(int userId);

        Task<IEnumerable<ReadDto>> GetReadThread(string bookTitle);
        Task<PagedList<ReadDto>> GetReadsForUser(FollowsParams readsParams);

    }
}