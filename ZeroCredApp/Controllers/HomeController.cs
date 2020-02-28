using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZeroCredApp.Models;
using ZeroCredApp.Service;
using ZeroCredApp.Settings;

namespace ZeroCredApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDemoService _demoService;
        private readonly DemoSettings _settings;
        private readonly IConfiguration _config;

        public HomeController(IDemoService demoService, IOptionsSnapshot<DemoSettings> demoSettings, ILogger<HomeController> logger, IConfiguration config)
        {
            _demoService = demoService;
            _settings = demoSettings.Value;
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult KeyVault()
        {
            var model = new KeyVaultConfigViewModel
            {
                SecretValue = _config[_settings.KeyVaultSecret]
            };
            return View(model);
        }
        public async Task<IActionResult> SqlData()
        {
            var books = await _demoService.AccessSqlDatabase();
            return View(books);
        }
        public async Task<IActionResult> Storage()
        {
            StorageViewModel model = await _demoService.AccessStorage();
            return View(model);
        }
        [HttpGet]
        public IActionResult EventHubsSend(string m) 
        {
            if (m != null) ViewBag.EventhubSuccess = "true";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EventHubsSend(EventHubMessage eventHubMessage)
        {
            await _demoService.SendEventHubsMessage(eventHubMessage.Message);
            return RedirectToAction(nameof(EventHubsSend), new { m="true"});
        }
        [HttpGet]
        public async Task<IActionResult> ServiceBusListen() => View();

        [HttpGet]
        public async Task<IActionResult> EventHubsListen() => View();

        [HttpGet]
        public async Task<IActionResult> ServiceBusSend(string m) 
        {
            if (m != null) ViewBag.ServicebusSuccess = "true";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ServiceBusSend(ServicebusMessage serviceMessage)
        {
            await _demoService.SendServiceBusQueueMessage(serviceMessage.Message);
            return RedirectToAction(nameof(ServiceBusSend), new { m="true"});
        }
        public async Task<IActionResult> DataLake()
        {
            DataLakeViewModel model = await _demoService.AccessDataLake();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
