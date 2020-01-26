using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class SubCategory
    {
       
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Channel Channel { get; set; }
        public virtual IEnumerable<Thread> Threads { get; set; }
    }
}
