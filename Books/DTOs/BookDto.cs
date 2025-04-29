using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.DTOs
{
    public class BookDto
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
   
        public string Author { get; set; }
        public double Rating { get; set; }
        
         public string PhotoUrl { get; set; }
        public string AboutAuthor { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string Review { get; set; }
        public ICollection<BookPhotoDto> BPhotos { get; set; } 
        
    }
}