using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Helpers
{
    public class FollowsParams : PaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
    }
}