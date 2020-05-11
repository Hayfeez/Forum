using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class Thread
    {      

        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }
        public DateTime DateCreated { get; set; }

        public int CategoryId { get; set; }
        public long SubscriberUserId { get; set; }
        public  SubscriberUser SubscriberUser { get; set; }
        public  Category Category { get; set; }

        public ThreadInfo ThreadInfo { get; set; }
        public IEnumerable<ThreadHistory> ThreadHistories { get; set; }
        public  IEnumerable<ThreadReply> ThreadReplies { get; set; }

        public IEnumerable<UserFollowedThread> UserFollowedThreads { get; set; }
        public IEnumerable<UserFlaggedThread> UserFlaggedThreads { get; set; }
        public IEnumerable<UserBookmarkedThread> UserBookmarkedThreads { get; set; }
    }
}
