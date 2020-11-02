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
using Shared.DTOs;
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
        public async Task<IActionResult> Index(int? limit, int? page)
        {
            limit = limit ?? 20;
            page = page ?? 0;
            string UserId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var result = await myAppClient.ListAll(UserId, limit, page);
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
                    if (result)
                    {
                        return Json(new { Code = 200, Message = "Sucess", Id = appClient.AppID });
                    }
                    else
                    {
                        return Json(new { Code = 400, Message = "Fail" });
                    }
                 
                }
                catch (Exception ex)
                {
                    return Json(new { Code = 501, Message = ex.Message });
                }
               
            }
            return Json(new { Code = 400, Message = "Your input is null" });
        }

        public async Task<IActionResult> Edit(AppClientDto appClient)
        {
            //Query from api KYC
            var resultClient = await apiKYC.QueryClient(appClient.AppID);

            AppClientDto appClientDto = new AppClientDto()
            {
                AppID = appClient.AppID,
                ClientId = resultClient.ClientId,
                ClientName = resultClient.ClientName,
                Description = resultClient.Description,
                Id = appClient.Id,
                Revision = appClient.Revision
            };

            UpdateClientModel model = new UpdateClientModel()
            {
               AppClientDto = appClientDto,
               ClientApiDto = resultClient
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateClientModel param)
        {
            var result = await myAppClient.UpdateAppClient(param.ClientApiDto, param.AppClientDto);
            if (result)
            {
                return Json(new { Code = 200, Message = "Update success." });
            }
            else
            {
                return Json(new { Code = 501, Message = "Update fail." });
            }
          
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
