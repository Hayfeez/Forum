using System;
using System.Collections.Generic;

namespace Forum.ViewModels
{
    public class HomeViewModel
    {
        public ThreadVM Guideline { get; set; }
        public IEnumerable<ThreadVM> Threads { get; set; }
    }
}
