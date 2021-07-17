using HopOn.Model;
using HopOn.Model.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HopOn.Core.Contract
{
    public interface IBackgroundServiceTest :IHostedService
    {
        Task<string> Test();
    }
}
