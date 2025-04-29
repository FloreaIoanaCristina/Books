using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.DTOs;
using Books.Entities;
using Books.Helpers;

namespace Books.Interfaces
{
    public interface IReview
    {
        void AddReview(Review review);
        
        void DeleteReview(Review review);
        Task<Review> GetReview(int id);
        Task<PagedList<ReviewDto>> GetReviewsForUser(ReviewParams reviewParams);
        Task<IEnumerable<ReviewDto>> GetReviewThread( string bookTitle);
        Task<bool> SaveAllAsync();
        
    }
}