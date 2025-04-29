using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Interfaces
{
    public interface IUnitOfWork
    {
        IGenreRepo GenreRepo { get; }
        Task<bool> SaveAsync();
        IBooks BookRepository  { get; }
    }
}