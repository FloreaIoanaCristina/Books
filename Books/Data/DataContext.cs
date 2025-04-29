using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Books.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
     IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
     IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AppBooks> BooksTable { get; set; }
        public DbSet<Genre> Genres {get; set; } 
       
        public DbSet<UserFollow> Followers { get; set; }
        public DbSet<FavoriteBook> FavoriteBooks { get; set; }
        public DbSet<BooksRead> BooksReads { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Read> Reads { get; set;}
        public DbSet<Message> Messages { get; set; }
         public DbSet<Photo> Photos { get; set; }   
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Group> Groups { get; set;}
        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

            builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

            builder.Entity<UserFollow>()
            .HasKey(k => new {k.SourceUserId, k.TargetUserId});

            builder.Entity<UserFollow>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.FollowedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollow>()
            .HasOne(s => s.TargetUser)
            .WithMany(l => l.FollowedByUsers)
            .HasForeignKey(s => s.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FavoriteBook>()
            .HasKey(k => new { k.SourceUserId, k.TargetBookId});

            builder.Entity<FavoriteBook>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedBooks)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BooksRead>()
            .HasKey(k => new { k.SourceUserId, k.TargetBookId});

            builder.Entity<BooksRead>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.ReadBooks)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
            .HasOne(u => u.Book)
            .WithMany(m => m.ReviewReceived)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.ReviewSent)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Read>()
            .HasOne(u => u.Book)
            .WithMany(m => m.ReadReceived)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Read>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.ReadSent)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Rating>()
            .HasOne(u => u.Book)
            .WithMany(m => m.RateReceived)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Rating>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.RateSent)
            .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}