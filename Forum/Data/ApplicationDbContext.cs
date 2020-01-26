using System;
using System.Collections.Generic;
using System.Text;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<ThreadReply> ThreadReplies { get; set; }
        
    }
}
