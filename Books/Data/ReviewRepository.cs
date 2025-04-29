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
    public class ReviewRepository : IReview
    
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ReviewRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
        }

        public void DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
        }

        public async Task<Review> GetReview(int id)
        {
            return await _context.Reviews
            .Include(u => u.Sender)
            .Include(u=> u.Book)
            .SingleOrDefaultAsync( x => x.Id == id);
        }

        public async Task<PagedList<ReviewDto>> GetReviewsForUser(ReviewParams reviewParams)
        {
            var query = _context.Reviews
            .OrderBy( m => m.ReviewSent)
            .AsQueryable();

            // query = reviewParams.Container switch 
            // {
            //     "Inbox" => query.Where(u => u.BookTitle == reviewParams.BookTitle) ,
            //     _ => query.Where(u => u.BookTitle == reviewParams.BookTitle && u.DateSent == null)
            // };

            var reviews = query.ProjectTo<ReviewDto>(_mapper.ConfigurationProvider);
            return await PagedList<ReviewDto>.CreateAsync(reviews, reviewParams.PageNumber, reviewParams.PageSize);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewThread( string bookTitle)
        {
            var reviews = await _context.Reviews
           .Include(u => u.Sender).ThenInclude( p => p.Photos)
           .Include(u => u.Book).ThenInclude(p => p.BPhotos)
           .Where(
            m => m.Book.Title == bookTitle 
        //    && m.Sender.UserName == currentUsername ||
        //     m.Book.Title == currentUsername && m.Sender.UserName == bookTitle
            
           
           )
           .OrderByDescending(m => m.ReviewSent)
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
           return _mapper.Map<IEnumerable<ReviewDto>>(reviews); 
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}