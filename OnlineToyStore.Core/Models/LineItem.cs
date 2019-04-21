using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace OnlineToyStore.Core.Models
{
    public class LineItem
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "productId")]
        public int ProductId { get; set; }
        [JsonProperty(PropertyName = "product")]
        public Product Product { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        public LineItem()
        {

        }
    }
}
