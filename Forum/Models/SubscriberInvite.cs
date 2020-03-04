using System;
namespace Forum.Models
{
    public class SubscriberInvite
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string InviteCode { get; set; }
        public DateTime DateCreated { get; set; }

        public int SubscriberId { get; set; }
        public Subscriber Subscriber{ get; set; }
    }
}
