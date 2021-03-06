﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Forum.Models
{
    public class Category
    {
       
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public int ChannelId { get; set; }
       [JsonIgnore] public  Channel Channel { get; set; }
        [JsonIgnore] public  IEnumerable<Thread> Threads { get; set; }
    }
}
