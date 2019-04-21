using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineToyStore.Core.Models;

namespace OnlineToyStore.Models
{
    public class OrderViewModel
    {
        public Order NewOrder { get; set; }
        public List<Product> AvailableProducts { get; set; }
    }
}
