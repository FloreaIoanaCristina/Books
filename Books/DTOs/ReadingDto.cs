using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.DTOs
{
    public class ReadingDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookPhotoUrl { get; set; }
        public DateTime ReadSent { get; set; } = DateTime.UtcNow;
    }
}