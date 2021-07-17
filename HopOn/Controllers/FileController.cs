using HopOn.Core.Contract;
using HopOn.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn.Controllers
{
    [Route("[controller]")]
    public class FileController : Controller
    {
        private IFileLinkService _fileLinkService;
        public FileController(IFileLinkService fileLinkService)
        {
            _fileLinkService = fileLinkService;
        }
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
