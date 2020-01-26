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
        public DateTime DateCreated { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        public virtual IEnumerable<ThreadReply> ThreadReplies { get; set; }
    }
}
