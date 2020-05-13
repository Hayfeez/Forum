using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    [Table("UserPeopleInfo")]
    public class UserPeopleInfo
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        public long SubscriberUserId { get; set; }
        public bool Followed { get; set; }

        public SubscriberUser SubscriberUser { get; set; }
        public IEnumerable<SubscriberUser> UserFollowing { get; set; }
    }
}
