using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SampleApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class InMemoryMiddleware
    {
        private readonly RequestDelegate _next;
        ILocalMemoryRepository _iLocMemRepo;
        IMemoryCache _iMemoryCache;
        public InMemoryMiddleware(RequestDelegate next, ILocalMemoryRepository iLocMemRepo, IMemoryCache iMemoryCache) //
        {
            _next = next;
            _iLocMemRepo = iLocMemRepo;
            _iMemoryCache = iMemoryCache;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //await _iLocMemRepo.GetItems(null);
            var obj = await _iLocMemRepo.GetItems(null);
            _iMemoryCache.Set("config_data", obj, TimeSpan.FromDays(1));
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class InMemoryMiddlewareExtensions
    {
        public static IApplicationBuilder UseInMemoryMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<InMemoryMiddleware>();
        }
    }
}
