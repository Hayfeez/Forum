using System;
namespace Forum.Models
{
    public class ResetPasswordCode
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string ResetCode { get; set; }
        public DateTime DateCreated { get; set; }

        public int SubscriberId { get; set; }
        public Subscriber Subscriber{ get; set; }
    }
}
