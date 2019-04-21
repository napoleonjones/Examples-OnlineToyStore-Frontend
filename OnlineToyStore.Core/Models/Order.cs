using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineToyStore.Core.Models
{
    public class Order
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "firstname")]
        public string Firstname { get; set; }

        [JsonProperty(PropertyName = "lastname")]
        public string Lastname { get; set; }

        [JsonProperty(PropertyName = "zipecode")]
        public string Zipcode { get; set; }

        [JsonProperty(PropertyName = "lineItems")]
        public IList<LineItem> LineItems { get; set; }

        [JsonProperty(PropertyName = "discount")]
        public decimal Discount { get; set; }

        [JsonProperty(PropertyName = "totalPrice")]
        public decimal TotalPrice { get; set; }

        [JsonProperty(PropertyName="orderDate")]
        public DateTimeOffset OrderDate { get; set; }

        public Order()
        {
            LineItems = new List<LineItem>();
        }
    }
}
