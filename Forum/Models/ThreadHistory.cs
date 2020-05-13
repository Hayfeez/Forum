using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Forum.Models
{
    [Table("ThreadHistory")]
    public class ThreadHistory
    {      
        public long Id { get; set; }
        public long ThreadId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        
    }
}
