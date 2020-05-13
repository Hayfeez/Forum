using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    [Table("UserThreadInfo")]
    public class UserThreadInfo
    {
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public long SubscriberUserId { get; set; }
        public bool Flagged { get; set; }
        public bool Bookmarked { get; set; }
        public bool Followed { get; set; }
        public bool Liked { get; set; }

        public Thread Thread { get; set; }
        public SubscriberUser SubscriberUser { get; set; }
    }
}
