using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace xamarinproject
{
    public class Product
    {
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; } // New property for the image URL
    }
}
