using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Redis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache_Poc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDistributedCache _iCache;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache iCache)
        {
            _logger = logger;
            _iCache = iCache;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost("GetCacheItem")]
        public async Task<List<MdlCacheResponse>> GetCacheItem()
        {
            List<MdlCacheResponse> objCacheItems = new List<MdlCacheResponse>();
            var encodeSerializedItems = await _iCache.GetAsync("config_data1");
            if (encodeSerializedItems != null)
            {
                var strItems = Encoding.UTF8.GetString(encodeSerializedItems);
                objCacheItems = JsonConvert.DeserializeObject<List<MdlCacheResponse>>(strItems);
            }
            else
            {
                //    objCacheItems.Add(new MdlCacheResponse
                //    {
                //        UserId = 1,
                //        Id = 1,
                //        Title = "T",
                //        Body = "B"
                //    });

                //List<MdlCacheResponse> objLmData = new List<MdlCacheResponse>();
                string endpoint = "https://jsonplaceholder.typicode.com/posts";
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(endpoint))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        objCacheItems = JsonConvert.DeserializeObject<List<MdlCacheResponse>>(apiResponse);
                    }
                }
                //return objLmData;


                var serializedItems = JsonConvert.SerializeObject(objCacheItems);
                var encodecItems = Encoding.UTF8.GetBytes(serializedItems);
                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddSeconds(10));
                await _iCache.SetAsync("config_data1", encodecItems, options);
            }

            return objCacheItems;
        }
    }
}
