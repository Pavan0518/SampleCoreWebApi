using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Models
{
    public class MdlCacheResponse
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
