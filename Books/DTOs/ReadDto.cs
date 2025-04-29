using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.DTOs
{
    public class ReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
   
        public string Author { get; set; }
        public string PhotoUrl { get; set; }
    }
}