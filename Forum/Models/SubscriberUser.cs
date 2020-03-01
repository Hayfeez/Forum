using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class SubscriberUser
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public double Rating { get; set; }
        public string HeaderImageUrl { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }


        public  ApplicationUser ApplicationUser { get; set; }
        public  Subscriber Subscriber { get; set; }

        public  IEnumerable<Thread> Threads { get; set; }
        public  IEnumerable<ThreadReply> ThreadReplies { get; set; }
    }
}
