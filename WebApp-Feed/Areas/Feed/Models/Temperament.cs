using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

// Id | Name     | BreedId
// ----------------------------------------
// 1  | Active   | 1
// 2  | Friendly | 1
// 3  | Vocal    | 2
// 4  | Social   | 2


namespace WebApp_Feed.Areas.Feed.Models
{
    public class Temperament
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // связь с породой
        public int BreedId { get; set; }
        public List<Breed> Breeds { get; set; } = new();

    }
}