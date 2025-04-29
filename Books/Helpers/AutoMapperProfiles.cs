using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.DTOs;
using Books.Entities;
using Books.Extensions;

namespace Books.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>().ForMember(dest => dest.PhotoUrl,
             opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
            .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<Review, ReviewDto>()
            .ForMember( d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.BookPhotoUrl, o => o.MapFrom(s => s.Book.BPhotos.FirstOrDefault(x => x.IsMain).UrlBook));
            CreateMap<Read, ReadingDto>()
            .ForMember( d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.BookPhotoUrl, o => o.MapFrom(s => s.Book.BPhotos.FirstOrDefault(x => x.IsMain).UrlBook));
            CreateMap<FavoriteBook, LikeDto>();
            CreateMap<BooksRead, ReadDto>();
             CreateMap<Rating, RatingDto>()
            .ForMember( d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.BookPhotoUrl, o => o.MapFrom(s => s.Book.BPhotos.FirstOrDefault(x => x.IsMain).UrlBook));
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}