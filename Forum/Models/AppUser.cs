using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class AppUser : IdentityUser
    {      

        public int SubscriberId { get; set; }
        public int Rating { get; set; }
        public string ProfileImageUrl { get; set; }
        public string HeaderImageUrl { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }

        public virtual IEnumerable<Thread> Threads { get; set; }
        public virtual IEnumerable<ThreadReply> ThreadReplies { get; set; }
       
    }
}
