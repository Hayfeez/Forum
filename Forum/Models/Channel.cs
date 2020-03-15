using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class Channel
    {
        public Channel()
        {
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string LogoUrl { get; set; }

        public int SubscriberId { get; set; }
        public  Subscriber Subscriber { get; set; }
        public  IEnumerable<Category> Categories { get; set; }
    }
}
