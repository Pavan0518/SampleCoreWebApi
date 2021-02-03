using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Redis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache_Poc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisCacheController : ControllerBase
    {
        private readonly IDistributedCache _iCache;
        public RedisCacheController(IDistributedCache iCache)
        {
            _iCache = iCache;
        }
        [HttpGet]
        public async Task<List<MdlCacheResponse>> GetCacheItem()
        {
            List<MdlCacheResponse> objCacheItems = null;
            string strSerializedItems = "";
            var encodeSerializedItems = await _iCache.GetAsync("config_data");
            if(encodeSerializedItems != null)
            {
                strSerializedItems = Encoding.UTF8.GetString(encodeSerializedItems);
            } else
            {

            }
            return objCacheItems;
        }
    }
}
