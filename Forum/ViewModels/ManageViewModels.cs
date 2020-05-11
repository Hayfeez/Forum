using System;
using System.Collections.Generic;
using Forum.Models;

namespace Forum.ViewModels
{
    public class UpdateUserRole
    {
        public string AppUserId { get; set; }
        public string Role { get; set; }
    }

    public class SaveSubscriberInfo
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Description { get; set; }
        public string HeaderImageUrl { get; set; }
        public string LogoImageUrl { get; set; }
        public bool AllowJoinNow { get; set; }
        public bool IsPublic { get; set; }
        public int FlagLimit { get; set; }
        public int DownvoteLimit { get; set; }

       // public int Id { get; set; }
    }

    public class ForumStatistics
    {
        public long Channels { get; set; }
        public long Categories { get; set; }
        public long Threads { get; set; }
        public long Users { get; set; }
        public int PendingInvites { get; set; }
    }

    public class TenantUsersList
    {
        public long Id { get; set; }
        public double Rating { get; set; }
        public string HeaderImageUrl { get; set; }
        public string ProfileImageUrl { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }

        public string ApplicationUserId { get; set; }

    }

    public class TenantInviteList
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string InviteCode { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class TenantSettings
    {
        public Subscriber Subscriber { get; set; }
        public IEnumerable<PinnedPost> PinnedPosts { get; set; }
        public IEnumerable<ChannelVM> Channels { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }
    }
}
