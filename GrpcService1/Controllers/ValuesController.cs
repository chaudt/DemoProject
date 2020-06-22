using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GrpcService1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Service1: call service 1 to get data");
            var results = new List<string>();
            /*
             * Kịch bản:
             *  + Service1 gọi service2
             *  + Service2 gọi ngược lại services
             */
            var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new GrpcService2.Greeter.GreeterClient(channel);
            using (var call = client.SayHello(new GrpcService2.HelloRequest  { Name = "Đoàn Tô Châu"}))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"{currentCustomer.Message}");

                    results.Add(currentCustomer.Message);
                }
            }
            return Ok(results);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
