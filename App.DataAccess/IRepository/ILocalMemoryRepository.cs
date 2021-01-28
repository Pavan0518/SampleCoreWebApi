using SampleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Repository
{
    public interface ILocalMemoryRepository
    {
        Task<List<LocalMemoryresponse>> GetItems(int? Id);
    }
}
