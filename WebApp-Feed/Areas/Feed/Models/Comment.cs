using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp_Feed.Areas.Feed.Models
{
    public class Comment
    {
    public int Id { get; set; }
    public string Text { get; set; }
    public string Author { get; set; }

    public int BreedId { get; set; }
    public Breed Breed { get; set; }
    }
}