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
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            this.context = context;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await this.context.Users.Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var search = userParams.Search;
            var sql = @"
               SELECT 
                   u.*, 
                   p.IsMain, p.Url
               FROM AspNetUsers u
               LEFT JOIN Photos p ON u.Id = p.AppUserId
            ";

            if (!string.IsNullOrEmpty(search))
            {
                sql += $" WHERE UserName LIKE '%{search}%'";
            }
            var users = await context.Users
             .FromSqlRaw(sql)
             .Include(x => x.Photos)
             .AsNoTracking()
             .ToListAsync();

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            // Filter in memory
            var filteredUsers = users
                .Where(u => u.UserName != userParams.CurrentUsername)
                .Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            // Sort
            filteredUsers = userParams.OrderBy == "created"
                ? filteredUsers.OrderByDescending(u => u.Created)
                : filteredUsers.OrderByDescending(u => u.LastActive);

            // Project into DTO
            var members = filteredUsers.Select(user => new MemberDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Age = CalculateAge(user.DateOfBirth),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Created = user.Created,
                LastActive = user.LastActive,
                Gender = user.Gender,
                Introduction = user.Introduction,
                LookingFor = user.LookingFor,
                Interests = user.Interests,
                City = user.City,
                Country = user.Country
            }).AsQueryable();

            // Then use your paginator
            return PagedList<MemberDto>.Create(members, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await this.context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await this.context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await this.context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            this.context.Entry(user).State = EntityState.Modified;
        }
        public async Task<AppUser> GetUserByPhotoId(int photoId)
        {
            return await this.context.Users
            .Include(p => p.Photos)
            .IgnoreQueryFilters()
            .Where(p => p.Photos.Any(p => p.Id == photoId))
            .FirstOrDefaultAsync();
        }
        private int CalculateAge(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return age;
        }
    }
}