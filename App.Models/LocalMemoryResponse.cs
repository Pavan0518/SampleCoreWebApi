using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Repository
{
    public class LocalMemoryresponse
    {

        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
         public string Body { get; set; }
       
    }
}
