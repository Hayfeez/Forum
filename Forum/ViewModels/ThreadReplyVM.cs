using System;

namespace Forum.ViewModels
{
    public class ThreadReplyVM
    {
        public long Id { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorImageUrl { get; set; }
        public int AuthorRating { get; set; }
        public DateTime DatePosted { get; set; }
        public string Content { get; set; }

        public long  ThreadId { get; set; }
    }
}