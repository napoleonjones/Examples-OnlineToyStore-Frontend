using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineToyStore.Core.Interfaces;
using OnlineToyStore.Core.Models;
using OnlineToyStore.Core.Repositories;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Diagnostics;

namespace OnlineToyStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private const string key = "";
        private const string QueueEndPoint = "DefaultEndpointsProtocol=https;AccountName=onlinetoystorestorage;AccountKey=AaiVztcTzXAZiqv7TDlpQJlSCeCQ91P2jT/sRS6iUAgW0odMZkuLiVnjuTrd0kznzoiobFFsfX9IZqG4rQ5cAg==;EndpointSuffix=core.windows.net";

        public IHttpClientFactory HttpClientFactory { get; set; }
        public IOrdersRepository<Order> OrdersRepository { get; set; }

        public OrdersController(IHttpClientFactory httpClientFactory, IOrdersRepository<Order> ordersRepository)
        {
            HttpClientFactory = httpClientFactory;

            OrdersRepository = ordersRepository;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult<Order>> CreateAsync(Order order)
        {
            Debug.WriteLine($"Sending Msg for {order.Id}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(QueueEndPoint);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference("onlinetoystore-orderprocessing");

            await queue.CreateIfNotExistsAsync();

            CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(order));

            await queue.AddMessageAsync(message);

            //await OrdersRepository.CreateItemAsync(order);

            return Ok(order);
        }
    }
}