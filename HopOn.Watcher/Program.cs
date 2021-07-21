using HopOn.Core.Contract;
using HopOn.Core.Services;
using HopOn.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Watcher
{
    public class Program
    {
        public IConfiguration Configuration { get; }

        public static void Main(string[] args)
        {
            var builder = new HostBuilder()
                 .ConfigureAppConfiguration((hostingContext, config) =>
          {
              config.AddEnvironmentVariables();

              if (args != null)
              {
                  config.AddCommandLine(args);
              }
          })
               .ConfigureServices((hostContext, services) =>
           {
               services.AddOptions();
               services.Configure<IFileHandler>(hostContext.Configuration.GetSection("FileHandler"));

               services.AddSingleton<IHostedService, DaemonService>();
               services.AddSingleton<IHostedService, QuotaDeamonService>();
           });
             builder.RunConsoleAsync();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                //.useSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddScoped<IFileHandler, AWSFileHandler>();
                    services.AddHostedService<Worker>();
                });
    }
}
