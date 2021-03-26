using HomeCinema.Domain;
using HomeCinema.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.DataAccess
{
    public class HomeCinemaDbContext : IdentityDbContext<User>
    {
        public HomeCinemaDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<UserActions> UserActions { get; set; }
        public DbSet<UserActionMovies> UserActionMovies { get; set; }
        //public DbSet<MovieRatings> MovieRatings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserActionMovies>().HasKey(op => new { op.UserActionId, op.MovieId });
            builder.Entity<User>().HasMany(u => u.UserActions).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            builder.Entity<Movie>().HasMany(m => m.UserActionMovies).WithOne(m => m.Movie).HasForeignKey(m => m.MovieId);
            builder.Entity<UserActions>().HasMany(a => a.UserActionMovies).WithOne(a => a.UserActions).HasForeignKey(a => a.UserActionId);
            //builder.Entity<MovieRatings>().HasKey(op => new { op.MovieId, op.UserId });
            string adminRoleId = Guid.NewGuid().ToString();
            string userRoleId = Guid.NewGuid().ToString();
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = userRoleId, Name = "user", NormalizedName = "USER" });
            string adminId = Guid.NewGuid().ToString();
            string userId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
                new User
                {
                    Id=adminId,
                    FullName = "Administrator",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "admin123"),
                    SecurityStamp = string.Empty
                },
                 new User
                 {
                     Id=userId,
                     FullName = "Marija Prosheva",
                     UserName = "mariche",
                     NormalizedUserName = "MARICHE",
                     Email = "mariche@gmail.com",
                     NormalizedEmail = "MARICHE@GMAIL.COM",
                     EmailConfirmed = true,
                     PasswordHash = hasher.HashPassword(null, "mariche123"),
                     SecurityStamp = string.Empty
                 });
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId=adminRoleId,UserId=adminId},
                new IdentityUserRole<string> { RoleId=userRoleId,UserId=userId});
            builder.Entity<Movie>().HasData(
                new Movie { Id = 1, Name = "Joker", Genre = "Adventure" ,OriginalId=33},
                new Movie {Id=2,Name="The Lion King",Genre= "Animation" ,OriginalId=34 },
                new Movie {Id=3,Name="Paracite",Genre="Comedy",OriginalId=55 } );
            builder.Entity<UserActions>().HasData(
                new UserActions {Id=1, Action=Domain.Enums.Actions.upVote,UserId=userId},
                new UserActions {Id=2,Action=Domain.Enums.Actions.view,UserId=userId },
                new UserActions {Id=3,Action=Domain.Enums.Actions.download,UserId=userId });
            builder.Entity<UserActionMovies>().HasData(
                new UserActionMovies {MovieId=1,UserActionId=1 },
                new UserActionMovies {MovieId=1,UserActionId=2 },
                new UserActionMovies { MovieId=2,UserActionId=2},
                new UserActionMovies { MovieId=2,UserActionId=3}
                );
            //builder.Entity<MovieRatings>().HasData(
            //    new MovieRatings { MovieId=3,UserId=userId,Rating=1.5},
            //    new MovieRatings { MovieId=2,UserId=userId,Rating=3.5}
            //    );


        }
    }
}
