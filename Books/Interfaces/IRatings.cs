using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.DTOs;
using Books.Helpers;
using Books.Entities;

namespace Books.Interfaces
{
    public interface IRatings
    {
        void AddRate(Rating rate);
        
        void DeleteRate(Rating rate);
        Task<Rating> GetRate(int id);
        Task<PagedList<RatingDto>> GetRatingsForUser(ReviewParams reviewParams);
        Task<IEnumerable<RatingDto>> GetRatingThread( string bookTitle);
        Task<bool> SaveAllAsync();
    }
}