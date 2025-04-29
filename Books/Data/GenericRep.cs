using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class GenericRep<T> : IGeneric<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        public GenericRep(DataContext context)
        {
            _context = context;
            
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync();
        }

       

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}