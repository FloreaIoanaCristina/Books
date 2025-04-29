using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Helpers
{
    public class UserParams : PaginationParams
    {
      
        
        public string CurrentUsername { get; set; }
        public int MinAge { get; set; } = 12;
        public int MaxAge { get; set; } = 100;
        public string OrderBy { get; set; } = "lastActive";
        public string Search { get; set; } = null;
    }
}