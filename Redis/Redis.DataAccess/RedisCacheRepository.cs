﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis.DataAccess.IRepository;
using Redis.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Redis.DataAccess
{
    public class RedisCacheRepository : IRedisCacheRepository
    {
        private readonly IDistributedCache _iCache;
        public RedisCacheRepository(IDistributedCache iCache)
        {
            _iCache = iCache;
        }
        public async Task<List<MdlCacheResponse>> GetCacheItems()
        {
            List<MdlCacheResponse> objItems;
            string serializedItems;
            var encodedItmes = await _iCache.GetAsync("config_data");
            if (encodedItmes != null)
            {
                serializedItems = Encoding.UTF8.GetString(encodedItmes);
                objItems = JsonConvert.DeserializeObject<List<MdlCacheResponse>>(serializedItems);
            }
            else
            {
                string endpoint = "https://jsonplaceholder.typicode.com/posts";
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(endpoint))
                    {
                        serializedItems = await response.Content.ReadAsStringAsync();
                        objItems = JsonConvert.DeserializeObject<List<MdlCacheResponse>>(serializedItems);
                        encodedItmes = Encoding.UTF8.GetBytes(serializedItems);
                        var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)).SetAbsoluteExpiration(DateTime.Now.AddHours(24));
                    }
                }
            }
            return objItems;
        }
    }
}
