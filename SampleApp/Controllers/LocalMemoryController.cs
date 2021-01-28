using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SampleApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace SampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalMemoryController : ControllerBase
    {
        IMemoryCache _iMemoryCache;
        public LocalMemoryController(IMemoryCache iMemoryCache)
        {
            _iMemoryCache = iMemoryCache;
        }
        [HttpGet("GetLocalMemory")]
        public  List<LocalMemoryresponse> Get()
        {
            var objData = _iMemoryCache.Get<List<LocalMemoryresponse>>("config_data");
            return objData;
        }
        [HttpPost("UpdateInMemory")]
        public List<LocalMemoryresponse> Post(LocalMemoryresponse objInput)
        {
            List<LocalMemoryresponse> objData =  _iMemoryCache.Get<List<LocalMemoryresponse>>("config_data");
            objData.Add(objInput);
            _iMemoryCache.Set("config_data", objData, TimeSpan.FromDays(1));
            return _iMemoryCache.Get<List<LocalMemoryresponse>>("config_data");

        }
        
    }
}
