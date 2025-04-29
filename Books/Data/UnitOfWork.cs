using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.Interfaces;

namespace Books.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataBooks;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext dataBooks, IMapper mapper)
        {
            _mapper = mapper;
            _dataBooks = dataBooks;
            
        }

        public IGenreRepo GenreRepo => new GenreRepo(_dataBooks);

        public IBooks BookRepository => new BookRepository(_dataBooks, _mapper);

        public async Task<bool> SaveAsync()
        {
            return await _dataBooks.SaveChangesAsync() > 0;
        }
        
    }
}