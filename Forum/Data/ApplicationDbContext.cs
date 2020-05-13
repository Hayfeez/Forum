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
            
         //   this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subscriber> Tenants { get; set; }
        public DbSet<SubscriberUser> SubscriberUsers { get; set; }
        public DbSet<SubscriberInvite> SubscriberInvites { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<ThreadReply> ThreadReplies { get; set; }
        public DbSet<ResetPasswordCode> ResetPasswordCodes { get; set; }
        public DbSet<PinnedPost> PinnedPosts { get; set; }

        public DbSet<ThreadInfo> ThreadInfos { get; set; }
        public DbSet<ThreadReplyInfo> ThreadReplyInfos { get; set; }
        public DbSet<ThreadHistory> ThreadHistories { get; set; }

        public DbSet<UserThreadInfo> UserThreadInfos { get; set; }
        public DbSet<UserPeopleInfo> UserPeopleInfos { get; set; }
    }
}
