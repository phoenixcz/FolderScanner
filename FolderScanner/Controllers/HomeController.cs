using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FolderScanner.Interfaces;
using FolderScanner.Models;

namespace FolderScanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScanFolderOrchestrator _scanFolderOrchestrator;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IScanFolderOrchestrator scanFolderOrchestrator,
            ILogger<HomeController> logger
        )
        {
            _scanFolderOrchestrator = scanFolderOrchestrator;
            _logger = logger;
        }

        public IActionResult Index(string? path)
        {
            if (path == null)
            {
                return View("index", new ScanFolderModel {Path = "C:\\"});
            }

            try
            {
                var result = _scanFolderOrchestrator.ScanFolder(path);
                return View(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while scanning folder {Path}", path);
                return View("index", new ScanFolderModel
                {
                    Path = path,
                    Messages = new[]
                    {
                        "Oops something went wrong",
                        e.Message
                    },
                });
            }
        }

        public IActionResult Limitations()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}