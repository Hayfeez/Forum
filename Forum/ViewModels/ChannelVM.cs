using System;
using System.Collections.Generic;
using Forum.Models;

namespace Forum.ViewModels
{
    public class ChannelVM
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public IEnumerable<SubCategoryVM> SubCategory { get; set; }

        public int SubCategoryCount { get; set; }
    }


    public class SubCategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ThreadVM> Threads { get; set; }

        public int ThreadCount { get; set; }
        public ChannelVM Channel { get; set; }
    }
}
