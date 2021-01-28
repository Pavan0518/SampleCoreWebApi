using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using SampleApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace SampleApp.Repository
{
    public class LocalMemoryRepository : ILocalMemoryRepository
    {
       
         public async Task<List<LocalMemoryresponse>> GetItems(int? Id)
        {
            List<LocalMemoryresponse> objLmData = new List<LocalMemoryresponse>();
            string endpoint = "https://jsonplaceholder.typicode.com/posts";
            if (Id != null)
            {
                endpoint = "https://jsonplaceholder.typicode.com/posts/1";
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(endpoint))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    objLmData = JsonConvert.DeserializeObject<List<LocalMemoryresponse>>(apiResponse);
                    //_iMemoryCache.Set("configData", objLmData);
                }
            }
          return objLmData;
        }
    }
}