using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Books.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        [Range(1,5)]
        public int Rate { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public AppUser Sender { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public  AppBooks Book { get; set; }
        public DateTime RateSent { get; set; } = DateTime.UtcNow;

    }
}