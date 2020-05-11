using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    [Table("UserFollowedThread")]
    public class UserFollowedThread
    {      
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public long SubscriberUserId { get; set; }
        public DateTime DateCreated { get; set; }

        //public IEnumerable<Thread> Threads { get; set; }
        //public IEnumerable<SubscriberUser> SubscriberUsers { get; set; }
    }
}
