using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookPhotoUrl { get; set; }
        [Range(1,5)]
        public int Rate { get; set; }
         public DateTime RateSent { get; set; } = DateTime.UtcNow;
      
    }
}