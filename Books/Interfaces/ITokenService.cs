using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Entities;

namespace Books.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}