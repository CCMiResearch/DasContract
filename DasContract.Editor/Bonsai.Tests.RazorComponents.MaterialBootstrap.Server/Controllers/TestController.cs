using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bonsai.Tests.RazorComponents.MaterialBootstrap.Server.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [RequestSizeLimit(300_000_000)]
        public string File(List<IFormFile> files)
        {
            return files.Count + " files accepted";
        }
    }
}
