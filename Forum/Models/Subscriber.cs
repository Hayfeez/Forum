using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class Subscriber
    {      
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string HeaderImageUrl { get; set; }

        public virtual IEnumerable<AppUser> AppUsers { get; set; }
        public virtual IEnumerable<Channel> Channels { get; set; }
    }
}
