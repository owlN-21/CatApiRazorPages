using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp_Feed.Areas.Feed.Models;
using System.ComponentModel.DataAnnotations;


namespace WebApp_Feed.Areas.Feed.Models
{
    public class Breed
    {
        public int? Id { get; set; }  
        public string? ApiId { get; set; }  

        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Описание обязательно")]
        public string Description { get; set; }

        public ICollection<Image> Images { get; set; }
    }

}