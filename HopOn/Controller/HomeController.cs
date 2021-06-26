using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn
{
    public class HomeController : ControllerBase
    {
        public HomeController(IMemoryCache memoryCache)
        {

        }
        public async Task<ActionResult> SaveFileNameLocalStorage(List<IFormFile> files)
        {
            return new JsonResult(true);
        }
    }
}
