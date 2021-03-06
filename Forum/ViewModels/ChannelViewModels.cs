﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Forum.Models;

namespace Forum.ViewModels
{

    public class ChannelThreadCount
    {
        public int ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public long ThreadCount { get; set; }
    }

    #region List

    public class ChannelList
    {
        public IEnumerable<ChannelVM> Channels { get; set; }

    }

    public class CategoryList
    {
        public IEnumerable<CategoryVM> Categories { get; set; }

    }
    #endregion
    
    #region Single
    public class ChannelVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }

        public int CategoryCount { get; set; }
        public int ThreadCount { get; set; }
    }

    public class CategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ThreadVM> Threads { get; set; }

        public int ThreadCount { get; set; }
        public ChannelVM Channel { get; set; }
    }
    #endregion

    #region Add/Update

    public class SaveChannelVM
    {
        public int Id { get; set; }
        [Required]public string Title { get; set; }
        [Required]public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int SubscriberId { get; set; }
    }

    public class SaveCategoryVM
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        [Required] public int ChannelId { get; set; }
    }

    #endregion

}
