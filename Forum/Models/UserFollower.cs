using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    [Table("UserFollower")]
    public class UserFollower
    {
        public long Id { get; set; }
        public long SubscriberUserId { get; set; }
        public long PersonId { get; set; } 

        public SubscriberUser SubscriberUser { get; set; }
     //   public SubscriberUser Person { get; set; }
    }
}
