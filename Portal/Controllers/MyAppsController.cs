using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CouchDBService;
using DotNetOpenAuth.InfoCard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IIdentityResource identityResource;
        private readonly IGrantType grantType;
        private readonly IClientSecret clientSecret;
        private ICouchContext couchContext;
        private DBConfig dBConfig;

        public MyAppsController(ILogger<MyAppsController> logger, DBConfig dBConf)
        {
            _logger = logger;
            this.couchContext = new CouchContext();
            this.dBConfig = dBConf;

            apiKYC = new APILaoKYC();

            myAppClient = new MyAppClient(couchContext, dBConfig, apiKYC);
            identityResource = new IdentityResource(couchContext, dBConfig, apiKYC);
            grantType = new MyGrantType(couchContext, dBConfig, apiKYC);
            clientSecret = new MyClientSecret(couchContext, dBConfig, apiKYC);
        }
        public async Task<IActionResult> Index(int? limit, int? page)
        {
            limit = limit ?? 20;
            page = page ?? 0;
            string UserId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var result = await myAppClient.ListAll(UserId, limit, page).ConfigureAwait(false);
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
                    var result = await myAppClient.CreateAppClient(appClient).ConfigureAwait(false);
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
            var resultClient = await apiKYC.QueryClient(appClient.AppID).ConfigureAwait(false); //Query Client
            
            var resultIdentityRes = await identityResource.ListAll().ConfigureAwait(false); //Get Identity resource from couchDB
            var resultGrantType = await grantType.ListAll().ConfigureAwait(false); //Get Grant type from couchDB

            List<SelectListItem> allowedScopes = new List<SelectListItem>();
            List<SelectListItem> redirectUris = new List<SelectListItem>();
            List<SelectListItem> allowedGrantType = new List<SelectListItem>();
            List<SelectListItem> postLogoutRedirectUris = new List<SelectListItem>();
            List<SelectListItem> IdentityRetrict = new List<SelectListItem>();

            #region App client
            AppClientDto appClientDto = new AppClientDto()
            {
                AppID = appClient.AppID,
                ClientId = resultClient.ClientId,
                ClientName = resultClient.ClientName,
                Description = resultClient.Description,
                Id = appClient.Id,
                Revision = appClient.Revision
            };
            #endregion

            #region Allow scope
            foreach (var item in resultIdentityRes)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = item.displayName;
                selectListItem.Value = item.name;

                if (resultClient.AllowedScopes.Where(x => x.Equals(item.name)).Count() > 0)
                {
                    selectListItem.Selected = true;
                }
                allowedScopes.Add(selectListItem);
            }
            #endregion

            #region Allow grant type
            foreach (var item in resultGrantType)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = item.Text;
                selectListItem.Value = item.Value;
                if (resultClient.AllowedGrantTypes.Where(x => x.Equals(item.Value)).Count() > 0)
                {
                    selectListItem.Selected = true;
                }
                allowedGrantType.Add(selectListItem);
            }

            #endregion

            #region RedirectUris
            foreach (var item in resultClient.RedirectUris)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = item.ToString();
                selectListItem.Value = item.ToString();
                selectListItem.Selected = true;
                redirectUris.Add(selectListItem);
            }
            #endregion

            #region PostLogoutRedirectUris
            foreach (var item in resultClient.PostLogoutRedirectUris)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = item.ToString();
                selectListItem.Value = item.ToString();
                selectListItem.Selected = true;
                postLogoutRedirectUris.Add(selectListItem);
            }
            #endregion

            #region 
            foreach (var item in resultClient.IdentityProviderRestrictions)
            {
                SelectListItem selectListItem = new SelectListItem();
                selectListItem.Text = item.ToString();
                selectListItem.Value = item.ToString();
                selectListItem.Selected = true;
                IdentityRetrict.Add(selectListItem);
            }
            #endregion

            UpdateClientModel model = new UpdateClientModel()
            {
                AppClientDto = appClientDto,
                ClientApiDto = resultClient,
                allowedScopes = allowedScopes,
                redirectUris = redirectUris,
                allowedGrantTypes = allowedGrantType,
                PostLogoutRedirectUris = postLogoutRedirectUris,
                IdentityRetrict = IdentityRetrict
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateClientModel param)
        {
            var result = await myAppClient.UpdateAppClient(param.ClientApiDto, param.AppClientDto).ConfigureAwait(false);
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
                    var result = await myAppClient.RemoveClientApp(id: id, rev: rev).ConfigureAwait(false);
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


        public async Task<IActionResult> Secret(int? limit, int? page, int? id)
        {
            limit = limit ?? 20;
            page = page ?? 0;
            string UserId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            ViewData["AppID"] = id;

            var result = await clientSecret.ListAll(id,UserId, limit, page).ConfigureAwait(false);
            return View(result.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Secret(ClientSecretDto appClient)
        {
            if (appClient != null)
            {
                try
                {
                    string UserId = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                    appClient.UserID = UserId;
                    appClient.Created = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                    if (appClient.expiration != "")
                    {
                        var expireDate = DateTime.ParseExact(appClient.expiration, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        appClient.expiration = $"{expireDate.ToUniversalTime():s}Z";
                    }
                    var result = await clientSecret.CreateClientSecret(appClient).ConfigureAwait(false);
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

        [HttpPost]
        public async Task<IActionResult> DeleteSecret(string id, string rev,string scid)
        {
            if (id != null && id != "")
            {
                try
                {
                    var result = await clientSecret.RemoveSecret(id: id, rev: rev, scid).ConfigureAwait(false);
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
