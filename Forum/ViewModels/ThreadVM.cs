using System;
using System.Collections.Generic;

namespace Forum.ViewModels
{
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
        public IEnumerable<ThreadReplyVM> Replies { get; set; }

        public int RepliesCount { get; set; }
        public SubCategoryVM SubCategory { get; set; }
    }
}
