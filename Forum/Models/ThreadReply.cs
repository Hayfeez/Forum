using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class ThreadReply
    {      

        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Thread Thread { get; set; }

    }
}
