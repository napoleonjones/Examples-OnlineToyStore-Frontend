using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineToyStore.Core.Models
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "unitPrice")]
        public decimal UnitPrice { get; set; }

        public Product()
        {

        }
    }
}
