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
    public class RatingRepo : IRatings
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RatingRepo(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void AddRate(Rating rate)
        {
            _context.Ratings.Add(rate);
        }

        public void DeleteRate(Rating rate)
        {
            _context.Ratings.Remove(rate);
        }

        public async Task<Rating> GetRate(int id)
        {
            return await _context.Ratings
            .Include(u => u.Sender)
            .Include(u => u.Book)
            .SingleOrDefaultAsync( x => x.Id == id);
        }

        public async Task<PagedList<RatingDto>> GetRatingsForUser(ReviewParams reviewParams)
        {
            var query =  _context.Ratings
            .OrderBy ( m => m.RateSent)
            .AsQueryable();

            var rating  = query.ProjectTo<RatingDto>(_mapper.ConfigurationProvider);
            return await PagedList<RatingDto>.CreateAsync(rating, reviewParams.PageNumber, reviewParams.PageSize);
        }

        public async Task<IEnumerable<RatingDto>> GetRatingThread(string bookTitle)
        {
            var rating = await _context.Ratings
            .Include( u => u.Sender).ThenInclude(p => p.Photos)
            .Include( u => u.Book).ThenInclude(p => p.BPhotos)
            .Where(
                m => m.Book.Title ==  bookTitle
            ).OrderByDescending(m => m.RateSent)
            .ToListAsync();

            return _mapper.Map<IEnumerable<RatingDto>>(rating);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}