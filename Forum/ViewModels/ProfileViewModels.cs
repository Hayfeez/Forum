using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.ViewModels
{

    public class UserPeopleInfoVM
    {
        public long SubscriberUserId { get; set; }

        public long PersonId { get; set; }
        public string PersonName { get; set; }

        public string PersonImageUrl { get; set; }
        public DateTime DatePersonJoined { get; set; }

    }


    public class ProfileVM
    {
        public string ApplicationUserId { get; set; }
        public long SubscriberUserId { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string ProfileImageUrl { get; set; }
        
        public DateTime DateJoined { get; set; }
        public double Rating { get; set; }

        public int ThreadCount { get; set; }
        public int ReplyCount { get; set; }
        public int FollowerCount { get; set; }
        public int FollowingCount { get; set; }

        public int Flags { get; set; }
        public int Bookmarks { get; set; }
        public int Follows { get; set; }
        public int Likes { get; set; }

    }


}
