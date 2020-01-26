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

        public virtual Subscriber Subscriber { get; set; }
        public virtual IEnumerable<SubCategory> SubCategories { get; set; }
    }
}
