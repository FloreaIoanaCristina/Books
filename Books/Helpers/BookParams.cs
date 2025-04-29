using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Helpers
{
    public class BookParams : PaginationParams
    {
        public int GenreId { get; set; }
        public string Title { get; set; }
    }
}