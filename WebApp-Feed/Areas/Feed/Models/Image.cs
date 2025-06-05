using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Id | Url       | BreedId
// ----------------------------------------
// 1  | cat1.jpg  | 1
// 2  | cat2.jpg  | 1
// 3  | cat3.jpg  | 2


namespace WebApp_Feed.Areas.Feed.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? ReferenceImageId { get; set; }
        public string? Url { get; set; }

        public int BreedId { get; set; }
        public Breed Breed { get; set; }
    }
}