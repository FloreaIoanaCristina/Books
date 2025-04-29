using System.ComponentModel.DataAnnotations;
using Books.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Books.Entities
{
    public class AppUser : IdentityUser<int>
    {
       
        public DateOnly DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } =DateTime.UtcNow;

        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public List<Photo> Photos { get; set; } = new();
        public List<UserFollow> FollowedByUsers { get; set; }
        public List<FavoriteBook> LikedBooks { get; set; }
        public List<BooksRead> ReadBooks { get; set; }
        public List<UserFollow> FollowedUsers { get; set; }
         public List<Message> MessagesSent { get; set; }
         public List<Message> MessagesReceived { get; set; }

         public ICollection<AppUserRole> UserRoles { get; set; }
         public ICollection<FavoriteBook> FavoriteBooks { get; set; } = new List<FavoriteBook>();
         public ICollection<BooksRead> BooksReads { get; set; } = new List<BooksRead>();

        public List<Review> ReviewSent { get; set; }
        public List<Read> ReadSent { get; set; }
        public List<Rating> RateSent { get; set; }
        
        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }
        
    }
}