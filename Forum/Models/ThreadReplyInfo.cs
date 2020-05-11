using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    [Table("ThreadReplyInfo")]
    public class ThreadReplyInfo
    {      
        public long Id { get; set; }
        public long ThreadReplyId { get; set; }
        public int Upvote { get; set; }
        public int Downvote { get; set; }
        public int Shares { get; set; }
        
        public  ThreadReply ThreadReply { get; set; }
    }
}
