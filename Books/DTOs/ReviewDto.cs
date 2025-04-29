using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Books.DTOs
{
    public class ReviewDto
    {
         public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookPhotoUrl { get; set; }
        public string Content { get; set; }
        [Range(1,5)]
        public int Rate { get; set; }
        public  DateTime? DateSent { get; set; }
        public DateTime ReviewSent { get; set; }
    }
}