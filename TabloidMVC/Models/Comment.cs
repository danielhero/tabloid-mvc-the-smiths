﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post post { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile userProfile { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
