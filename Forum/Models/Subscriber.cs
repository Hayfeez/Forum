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
        public bool AllowJoinNow { get; set; }

        public  IEnumerable<SubscriberUser> SubscriberUsers { get; set; }
        public  IEnumerable<Channel> Channels { get; set; }
    }
}
