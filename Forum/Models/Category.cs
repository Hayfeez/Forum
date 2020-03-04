﻿using System;
using System.Collections.Generic;

namespace Forum.Models
{
    public class Category
    {
       
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public int ChannelId { get; set; }
        public  Channel Channel { get; set; }
        public  IEnumerable<Thread> Threads { get; set; }
    }
}
