using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CouchDBService;
using DotNetOpenAuth.InfoCard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portal.Repositories;
using Shared.Configs;
using Shared.Entities;
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
            return View(result.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppClient appClient)
        {
            if (appClient != null)
            {
                try
                {
                    string UserId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                    appClient.UserID = UserId;
                    appClient.Created = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                    var result = await myAppClient.CreateAppClient(appClient);
                }
                catch (Exception)
                {
                    throw;
                }
               
            }
            return View();
        }

        public IActionResult Edit(string id, string rev)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id, string rev)
        {
            if (id != null && id != "")
            {
                try
                {
                    var result = await myAppClient.RemoveClientApp(id: id, rev: rev);
                    if (result)
                    {
                        return Json(new { Code = 200, Message = "Delete success." });
                    }
                    else
                    {
                        return Json(new { Code = 501, Message = "Delete fail." });
                    }
                }
                catch (Exception ex)
                {

                    return Json(new { Code = 501, Message = $"Delete fail: {ex.Message}." });
                }

            }
            return Json(new { Code = 404, Message = "ID is not incorrect." });
        }
    }
}
