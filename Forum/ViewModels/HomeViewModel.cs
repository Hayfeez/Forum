using System;
using System.Collections.Generic;
using Forum.Models;

namespace Forum.ViewModels
{
    public class HomeViewModel
    {
        public string SearchText { get; set; }

        public IEnumerable<PinnedPost> PinnedPosts { get; set; }
        public IEnumerable<ThreadVM> Threads { get; set; }
    }
}
