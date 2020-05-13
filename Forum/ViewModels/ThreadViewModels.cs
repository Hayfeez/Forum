using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Forum.Helpers;

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
        public ThreadInfoVM ThreadInfo { get; set; }
       
    }

    public class ThreadReplyVM
    {
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int AuthorRating { get; set; }
        public DateTime DatePosted { get; set; }
        public string Content { get; set; }
        public bool IsAuthorAdmin { get; set; }

        public ThreadReplyInfoVM ThreadReplyInfo { get; set; }
        public IEnumerable<long> ReplyInfoUserIds { get; set; }
    }

    public class ThreadInfoVM
    {
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public int Follows { get; set; }        
        public int Likes { get; set; }
        public int Flags { get; set; }
        public int Bookmarks { get; set; }
      //  public int Views { get; set; }

    }
 
    public class ThreadReplyInfoVM
    {
        public long Id { get; set; }
        public long ThreadReplyId { get; set; }
        public int Upvote { get; set; }
        public int Downvote { get; set; }
        public int Shares { get; set; }
       
    }


    public class UserThreadInfoVM
    {
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public long SubscriberUserId { get; set; }
        public bool Flagged { get; set; }
        public bool Bookmarked { get; set; }
        public bool Followed { get; set; }
        public bool Liked { get; set; }

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


        [Required]
        public int ChannelId { get; set; }
        [Required]
        public int CategoryId { get; set; }

    
        //public string ChannelName { get; set; }
      
        //public string CategoryName { get; set; }
        
    }

    public class SaveThreadReplyVM
    {
        public long Id { get; set; }
        [Required]
        public string Content { get; set; }

        public long ThreadId { get; set; }

    }

    public class SavePinnedPostVM
    {
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public bool IsActive { get; set; }

    }

    #endregion

    

    #region User Thread Actions

    public class SaveUserAction
    {
        public long ThreadId { get; set; }
        public long ThreadReplyId { get; set; }
        public long UserFollowingId { get; set; }
        public UserAction Action { get; set; }
    }

   
    #endregion
}
