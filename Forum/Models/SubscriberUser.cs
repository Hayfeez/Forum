using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class SubscriberUser
    {
        public long Id { get; set; }
        public double Rating { get; set; }
        public string ProfileImageUrl { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }

        public int SubscriberId { get; set; }
        public string ApplicationUserId { get; set; }

        public  ApplicationUser ApplicationUser { get; set; }
        public  Subscriber Subscriber { get; set; }

        public  IEnumerable<Thread> Threads { get; set; }
        public  IEnumerable<ThreadReply> ThreadReplies { get; set; }

        public IEnumerable<UserThreadInfo> UserThreadInfos { get; set; }

        public IEnumerable<UserFollower> UserFollowings { get; set; }
    }
}
