using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Contexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Remotion.Linq.Clauses.ResultOperators;

namespace CityInfo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //this is the method to use to enable logging at the Bootstrapping stage. We cannot inject as normal because the app has no knolwedge of it yet
            var logger = NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();

            try
            {
                
                logger.Info("initialising application...");
                var host = CreateWebHostBuilder(args).Build();

                using(var scope = host.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetService<CityInfoContext>();

                        //for demo purposes, delete db and migrate on startup so we start with a clean slate
                        context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error occured whilst migrating db");
                    }
                }

                host.Run();


            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application has stopped because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseNLog();
    }
}
