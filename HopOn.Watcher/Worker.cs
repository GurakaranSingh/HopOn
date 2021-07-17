using Amazon.XRay.Recorder.Core.Internal.Utils;
using HopOn.Core.Contract;
using HopOn.Model;
using HopOn.Model.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace HopOn.Watcher
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<DaemonConfig> _config;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        GetResponseFromAPI();
        //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //        await Task.Delay(1000, stoppingToken);
        //    }
        //}
        //private async Task GetResponseFromAPI()
        //{
        //    HttpResponseMessage response = await _Client.GetAsync("https://localhost:44341/api/Upload/Watcher");
        //}
    }
}
