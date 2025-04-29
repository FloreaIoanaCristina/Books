using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Books.Data;
using Microsoft.EntityFrameworkCore;

namespace Books.Entities
{
    
    public class AppBooks 
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        [Range(1,5)]
        public double Rating { get; set; }
        public string AboutAuthor { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public string Review { get; set; }
      //  public List<FavoriteBook> Liked { get; set; }
        public ICollection<BookPhoto> BPhotos { get; set; } = new List<BookPhoto>();
        public ICollection<FavoriteBook> FavoriteBooks { get; set; } = new List<FavoriteBook>();
        public ICollection<BooksRead> BooksReads { get; set; } = new List<BooksRead>();
        public List<Review> ReviewReceived { get; set; }
        public List<Read> ReadReceived { get; set; }
        public List<Rating> RateReceived { get; set; }
    }
}