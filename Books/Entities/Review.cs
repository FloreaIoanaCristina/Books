using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Books.Entities
{
    public class Review
    {
         public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public AppUser Sender { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public  AppBooks Book { get; set; }
        public string Content { get; set; }
        [Range(1,5)]
        public int Rate { get; set; }
        public  DateTime? DateSent { get; set; }
        public DateTime ReviewSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted{ get; set; }
       
    }
}