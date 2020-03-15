using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImageUrl { get; set; }

        public IEnumerable<SubscriberUser> SubscriberUsers { get; set; } //to turn off lazyloading, remove virtual
    }
   
}
