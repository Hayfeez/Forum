using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels
{

    #region List of Threads
    public class ChannelThreads
    {
        public ChannelVM Channel { get; set; }
        public IEnumerable<ThreadVM> Threads { get; set; }
    }

    public class CategoryThreads
    {

        public CategoryVM Category { get; set; }
        public IEnumerable<ThreadVM> Threads { get; set; }
    }

    #endregion


    #region Single Thread

    public class ThreadVM
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImageUrl { get; set; }
        public int AuthorRating { get; set; }
        public DateTime DatePosted { get; set; }
        public string Content { get; set; }
        public bool IsAuthorAdmin { get; set; }
        public string Tags { get; set; }
        public IEnumerable<ThreadReplyVM> Replies { get; set; }

        public int RepliesCount { get; set; }
        public CategoryVM Category { get; set; }
        public ChannelVM Channel { get; set; }
    }

    public class ThreadReplyVM
    {
        public long Id { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImageUrl { get; set; }
        public int AuthorRating { get; set; }
        public DateTime DatePosted { get; set; }
        public string Content { get; set; }
        public bool IsAuthorAdmin { get; set; }

        public ThreadVM Thread { get; set; }
    }

    #endregion


    #region Add/Update
    public class SaveThreadVM
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string Tags { get; set; }

        //public DateTime DateCreated { get; set; }

        [Required]
        public int ChannelId { get; set; }
        [Required]
        public int CategoryId { get; set; }

    
        //public string ChannelName { get; set; }
      
        //public string CategoryName { get; set; }

        public string UserId { get; set; }
        
    }

    public class SaveThreadReplyVM
    {
        public long Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        public long ThreadId { get; set; }
        public string UserId { get; set; }

    }


    #endregion

}
