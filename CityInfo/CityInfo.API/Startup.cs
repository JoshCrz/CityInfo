using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Contexts;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace CityInfo.API
{
    public class Startup
    {

        private readonly IConfiguration _configuration;

        //passing in iconfiguration so we can access connection strings in appsettings
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //add mvc to the project            
            services.AddMvc()
                .AddMvcOptions(o =>
                {                    
                    o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    //o.OutputFormatters.Clear();
                });

            //addJson options, below we are removing the default camelCase responses
            /*
            .AddJsonOptions(o =>
            {
                if (o.SerializerSettings.ContractResolver != null)
                {
                    var castedResolver = o.SerializerSettings.ContractResolver
                        as DefaultContractResolver;
                    castedResolver.NamingStrategy = null;
                }
            });*/


            //we can optionally pass either just the service.
            //services.AddTransient<LocalMailService>();

            //OR, we can pass the interface and the implementation
            //this is telling the app, whenever I inject IMailService, provide the LocalMailService
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif

            var connectionString = _configuration["connectionStrings:cityInfoDBConnectionStrings"];
            services.AddDbContext<CityInfoContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //configure repo's and services
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler();
            }

            //use status code pages, handle 404's etc (optional)
            app.UseStatusCodePages();

            //use mvc middleware
            app.UseMvc();

        }
    }
}
