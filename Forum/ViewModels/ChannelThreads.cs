using System;
using System.Collections.Generic;

namespace Forum.ViewModels
{
    public class ChannelThreads
    {
      
        public ChannelVM Channel { get; set; }
        public SubCategoryVM SubCategory { get; set; }
        public IEnumerable<ThreadVM> Threads { get; set; }
    }
}
