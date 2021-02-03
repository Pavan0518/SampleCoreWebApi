//using System;
//using System.Collections.Generic;
using System;
using System.Data;
//using System.Linq;
using System.Text;
using App.DataAccess.IRepository;
//using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using SampleApp.Filters.Exception;
using SampleApp.Middlewares;
//using SampleApp.Middlewares;
using SampleApp.Repository;

namespace SampleApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILocalMemoryRepository ILmRepo { get; set; }
        public IMemoryCache ImemoryCache { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMemoryCache();
            services.AddCors();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(CustomExceptionHandler));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IConfiguration>(Configuration);
            //services.AddSingleton<IMemoryCache>(ImemoryCache);
            services.AddTransient<IDbConnection>((sp) => new NpgsqlConnection(Configuration.GetConnectionString("DefaultConnection").ToString()));
            services.AddSingleton<ILoginRepository, LoginRepository>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<IUserSignUpRepository, UserSignUpRepository>();
            services.AddSingleton<ILocalMemoryRepository, LocalMemoryRepository>();
            services.AddSingleton<InMemoryMiddleware>();
            services.AddSwaggerGen(c =>
            {
                // configure SwaggerDoc and others

                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

            });
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = "localhost:6379";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime lifetime,
            ILocalMemoryRepository ilmr, 
            //IRedisCacheRepository iRcr,
            IMemoryCache imemoryCache) // 
        {
            ILmRepo = ilmr;
            ImemoryCache = imemoryCache;
            //app.UseInMemoryMiddleware();
            //ilmr.GetItems(1);
            //lifetime.ApplicationStarted.Register(OnApplicationStarted);
            //lifetime.ApplicationStarted.Register(() => iRcr.GetItems());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddLog4Net();
            app.UseCors(builder =>
           builder.WithOrigins("*")
               .AllowAnyHeader()
               .AllowAnyMethod());
            app.UseMvc();
            //app.UseExceptionHandler("/error");
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseAuthentication();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "Sample App";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });
        }

        public async void OnApplicationStarted()
        {
            //LocalMemoryRepository olmr = new LocalMemoryRepository();
            var objIConfigData = await ILmRepo.GetItems(null);
            ImemoryCache.Set("config_data", objIConfigData, TimeSpan.FromDays(1));
        }
        //public async void OnApplicationStartedRedis(IRedisCacheRepository iRcr)
        //{

        //}
    }
}
