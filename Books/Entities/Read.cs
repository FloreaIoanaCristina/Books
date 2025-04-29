using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Entities
{
    public class Read
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public AppUser Sender { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public  AppBooks Book { get; set; }
        public DateTime ReadSent { get; set; } = DateTime.UtcNow;
    }
}