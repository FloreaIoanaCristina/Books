using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Books.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public static string GetTitle(this ClaimsPrincipal book)
        {
            return book.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static int GetTitleId(this ClaimsPrincipal book)
        {
            return int.Parse(book.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}