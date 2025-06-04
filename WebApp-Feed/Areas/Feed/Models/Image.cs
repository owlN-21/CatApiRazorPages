using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_Feed.Areas.Feed.Models
{
    public class Image
    {
        public int? Id { get; set; }
        public string? ReferenceImageId { get; set; }
        public string? Url { get; set; }

        public int? BreedId { get; set; }
        public Breed Breed { get; set; }
    }
}