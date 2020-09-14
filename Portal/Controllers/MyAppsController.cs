using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CouchDBService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.Repositories;
using Shared.Configs;
using Shared.Repositories;

namespace Portal.Controllers
{
    [Authorize]
    public class MyAppsController : Controller
    {
        private readonly ILogger<MyAppsController> _logger;
        private readonly IAPILaoKYC apiKYC;
        private readonly IMyAppClient myAppClient;
        private ICouchContext couchContext;
        private DBConfig dBConfig;

        public MyAppsController(ILogger<MyAppsController> logger, DBConfig dBConf)
        {
            _logger = logger;
            this.couchContext = new CouchContext();
            this.dBConfig = dBConf;

            apiKYC = new APILaoKYC();

            myAppClient = new MyAppClient(couchContext, dBConfig, apiKYC);
        }
        public async Task<IActionResult> Index()
        {
            var result = await myAppClient.ListAll();
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

    }
}
