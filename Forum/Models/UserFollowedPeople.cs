using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    [Table("UserFollowedPeople")]
    public class UserFollowedPeople
    {      
        public long Id { get; set; }
        public long UserFollowingId { get; set; }
        public long UserFollowerId { get; set; }
        public DateTime DateCreated { get; set; }
       
        //public IEnumerable<SubscriberUser> FollowedUsers { get; set; }
        //public IEnumerable<SubscriberUser> SubscriberUsers { get; set; }
    }
}
