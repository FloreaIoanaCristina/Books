using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.DTOs;
using Books.Entities;

namespace Books.Helpers
{
    public class AutoMapperBooks : Profile
    {
        public AutoMapperBooks()
        {
            CreateMap<AppBooks, BookDto>()
            .ForMember(d => d.Genre, o => o.MapFrom(s => s.Genre.Name))
            .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.BPhotos.FirstOrDefault(p => p.IsMain).UrlBook));
         
            CreateMap<BookPhoto, BookPhotoDto>().ReverseMap();
            CreateMap<BookUpdateDto, AppBooks>();
            CreateMap<AddBookDto, AppBooks>().ReverseMap();
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<GenreDto, Genre>();
        }
    }
}