using SampleApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.DataAccess.IRepository
{
    public interface IRedisCacheRepository
    {
        Task<List<LocalMemoryresponse>> GetItems();
        void SetItems(List<LocalMemoryresponse> objItems);
    }
}
