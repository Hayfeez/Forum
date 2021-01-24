using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class PinnedPost
    {      

        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public int SubscriberId { get; set; }
        public  Subscriber Subscriber{ get; set; }
    }
}
