using System;
using System.Collections.Generic;

namespace Forum.ViewModels
{
    public class SearchViewModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<ThreadVM> Threads { get; set; }
        public int ResultCount { get; set; }
    }
}
