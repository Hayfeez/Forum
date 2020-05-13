using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class ThreadReply
    {      

        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }


        public long SubscriberUserId { get; set; }
        public long ThreadId { get; set; }
        public SubscriberUser SubscriberUser { get; set; }
        public Thread Thread { get; set; }
        public ThreadReplyInfo ThreadReplyInfo { get; set; }

        public IEnumerable<UserThreadReplyInfo> UserThreadReplyInfos { get; set; }

    }
}
