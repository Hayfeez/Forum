using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    [Table("UserThreadReplyInfo")]
    public class UserThreadReplyInfo
    {
        public long Id { get; set; }
        public long SubscriberUserId { get; set; }
        public long ThreadReplyId { get; set; }

        public SubscriberUser SubscriberUser { get; set; }
        public ThreadReply Reply { get; set; }
    }
}
