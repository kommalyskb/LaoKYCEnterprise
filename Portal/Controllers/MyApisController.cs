using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CouchDBService;
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
    public class MyApisController : Controller
    {
        private readonly ILogger<MyApisController> _logger;
        private readonly IAPILaoKYC apiKYC;
        private IMyAPIResource myAPIResouce;
        private ICouchContext couchContext;
        private DBConfig dBConfig;

        public MyApisController(ILogger<MyApisController> logger, DBConfig dBConf)
        {
            _logger = logger;
            this.couchContext = new CouchContext();
            this.dBConfig = dBConf;

            apiKYC = new APILaoKYC();

            myAPIResouce = new MyAPIResource(couchContext, dBConfig, apiKYC);
        }
        public async Task<IActionResult> Index()
        {
            var result = await myAPIResouce.ListAll();
            return View(result.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(APIResource resource)
        {
            if (resource != null)
            {
                try
                {
                    string UserId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                    
                    resource.UserID = UserId;
                    resource.Created = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                    var result = await myAPIResouce.CreateResource(resource);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return View();
        }

        public async Task<IActionResult> Edit(APIResource resource)
        {
            //Query from api KYC

            UpdateApiResource model = new UpdateApiResource()
            {
               
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateApiResource param)
        {
            var result = await myAPIResouce.UpdateResource(param.apiResourceApiDto, param.aPIResource);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id, string rev)
        {
            if (id != null && id != "")
            {
                try
                {
                    var result = await myAPIResouce.RemoveResource(id: id, rev: rev);
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
