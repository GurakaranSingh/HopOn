using HopOn.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HopOn.Core.Services
{
    public class BackgroundTestService : IBackgroundServiceTest
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Test()
        {
            string test = "sabdasjhd";
            return  test;
        }
    }
}
