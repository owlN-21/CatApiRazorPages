using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;

namespace WebApp_Feed.Areas.Feed.Models.ApiModels
{
    public class BreedApiModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Reference_Image_Id { get; set; }
        public string Temperament { get; set; }
    }

}