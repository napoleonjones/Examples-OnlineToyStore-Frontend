using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineToyStore.Models;
using OnlineToyStore.Core.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace OnlineToyStore.Controllers
{
    public class HomeController : Controller
    {
        public IHttpClientFactory HttpClientFactory { get; }

        public IList<Product> AvailableProducts { get; set; }

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;

            AvailableProducts = GetProducts();
        }

        public IActionResult Index()
        {
            var model = new Order
            {
                LineItems = GetLineItems()
            };
            var availableProducts = GetProducts();

            return View(model);
        }

        public async Task<IActionResult> CreateAsync(Order model)
        {
            model.Id = Guid.NewGuid();
            model.OrderDate = DateTimeOffset.UtcNow;

            ReHydrateProducts(model);

            var content = new StringContent(JsonConvert.SerializeObject(model).ToString(), Encoding.UTF8, "application/json");
            var client = HttpClientFactory.CreateClient("SubmitOrder");

            var result = await client.PostAsync("api/Orders", content);
            if (result.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Sent {model.Id}");
                return View("OrderSubmitted", model);
            }
            else
            {
                Debug.WriteLine(result.StatusCode);
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public async Task<IActionResult> OrderSubmitted(Order order)
        {
            return View(order);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Product> GetProducts()
        {
            var products = new List<Product>();
            var product1 = new Product
            {
                Id = 1,
                Name = "Slumber Girl",
                UnitPrice = 30M
            };

            var product2 = new Product
            {
                Id = 2,
                Name = "Plaid Boy",
                UnitPrice = 20M
            };

            products.Add(product1);
            products.Add(product2);

            return products;
        }

        public List<LineItem> GetLineItems()
        {
            var products = GetProducts();

            var lineItems = new List<LineItem>();
            var id = 1;
            foreach(var product in products)
            {
                lineItems.Add(new LineItem
                {
                    Id = id,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 1

                });

                id += 1;
            }

            return lineItems;
        }

        public void ReHydrateProducts(Order order)
        {
            foreach(var lineItem in order.LineItems)
            {
                if (lineItem.Product == null)
                {
                    lineItem.Product = AvailableProducts.Where(x => x.Id == lineItem.ProductId).First();
                }
            }
        }
    }
}
