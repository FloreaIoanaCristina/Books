using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Data;
using Books.Helpers;
using Books.Interfaces;
using Books.Services;
using Books.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Books.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt => 
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
           


            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IFollowers, FollowsRepository>();
            services.AddScoped<IBooks, BookRepository>();
            services.AddScoped(typeof(IGeneric<>),(typeof(GenericRep<>)));
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IGenreRepo, GenreRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILikes, LikesRepository>();
            services.AddScoped<IReview, ReviewRepository>();
            services.AddScoped<IRatings, RatingRepo>();
            services.AddScoped<IRead, ReadRepo>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IReading, ReadingRepository>();
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();

            return services;
        }
    }
}