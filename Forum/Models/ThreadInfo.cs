using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    [Table("ThreadInfo")]
    public class ThreadInfo
    {      
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public int Followers { get; set; }
        public int Viewers { get; set; }
        public int Likes { get; set; }
        public int Flags { get; set; }

        public  Thread Thread { get; set; }
    }
}
