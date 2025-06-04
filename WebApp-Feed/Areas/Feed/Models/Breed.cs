using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Feed.Areas.Feed.Models;

namespace WebApp_Feed.Areas.Feed.Models
{
    public class Breed
    {
        public int? Id { get; set; }  
        public string? ApiId { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Image> Images { get; set; }
    }

}