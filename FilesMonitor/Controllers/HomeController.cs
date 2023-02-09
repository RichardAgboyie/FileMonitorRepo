using System.Diagnostics;
using BusinessLogic.Interfaces;
using FilesMonitor.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilesMonitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISftpBusiness _sftpBusiness;

        public HomeController(ILogger<HomeController> logger, ISftpBusiness sftpBusiness)
        {
            _logger = logger;
            _sftpBusiness = sftpBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task Monitor()
        {
            await _sftpBusiness.MonitorFolder();
        }
    }
}