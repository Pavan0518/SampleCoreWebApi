using Redis.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redis.DataAccess.IRepository
{
    public interface IRedisCacheRepository
    {
        Task<List<MdlCacheResponse>> GetCacheItems();
    }
}
