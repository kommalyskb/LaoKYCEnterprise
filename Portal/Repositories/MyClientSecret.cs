using CouchDBService;
using Shared.Configs;
using Shared.DTOs;
using Shared.Entities;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Repositories
{
    public class MyClientSecret : IClientSecret
    {
        private readonly ICouchContext couchContext;
        private readonly DBConfig dBConfig;
        private readonly IAPILaoKYC apiLao;
        private readonly CouchDBHelper couchDbHelper;

        public MyClientSecret(ICouchContext couchContext, DBConfig dBConfig, IAPILaoKYC apiLao)
        {
            this.couchContext = couchContext;
            this.dBConfig = dBConfig;
            this.apiLao = apiLao;
            couchDbHelper = new CouchDBHelper(dBConfig.Scheme, dBConfig.SrvAddr,
                "clientsecret", dBConfig.Username, dBConfig.Password);
        }

        public async Task<bool> CreateClientSecret(ClientSecretDto appClient)
        {
           
            if (appClient.UserID == string.Empty)
            {
                return false;
            }

            var req = new ClientSecret()
            { description = appClient.description,expiration = appClient.expiration,type= appClient.type,value = appClient.value };

            var res = await apiLao.CreateClientSecret(appClient.AppID,req).ConfigureAwait(false);

            //res = ID Secret from identity
            if (res > 0)
            {
                appClient.SecretId = res;
                appClient.Id = null;
                var result = await couchContext.InsertAsync<ClientSecretDto>(couchDbHelper, appClient).ConfigureAwait(false);

                return result.IsSuccess;
            }
            return false;
        }

        public async Task<List<ClientSecretDto>> ListAll()
        {
            var result = await couchContext.ViewQueryAsync<ClientSecretDto>(
                    couchDBHelper: couchDbHelper,
                    designName: DesignName.AppSecretQuery,
                    viewName: IndexName.AppSecretQuery_List,
                    "none",
                    20,
                    0,
                    false,
                    false
                ).ConfigureAwait(false);

            if (result.Rows != null)
            {
                return result.Rows.Select(x => new ClientSecretDto()
                {
                    Id = x.Id,
                    Revision = x.Value.Revision,
                    UserID = x.Value.UserID,
                    description = x.Value.description,
                    expiration = x.Value.expiration,
                    SecretId = x.Value.SecretId,
                    type = x.Value.type,
                    value = x.Value.value
                }).ToList();
            }
            else
            {
                return new List<ClientSecretDto>();
            }
        }

        public async Task<List<ClientSecretDto>> ListAll(int? appid,string userId, int? limit, int? page)
        {
            if (userId == string.Empty)
            {
                return await Task.FromResult(new List<ClientSecretDto>()).ConfigureAwait(false);
            }

            int skip = limit.Value * page.Value;
            var result = await couchContext.ViewQueryAsync<ClientSecretDto>(
                    couchDBHelper: couchDbHelper,
                    designName: DesignName.AppSecretQuery,
                    viewName: IndexName.AppSecretQuery_List,
                    appid.ToString(),
                    limit.Value,
                    skip,// skip = page * limit
                    false,
                    false
                ).ConfigureAwait(false);
            if (result.Rows != null)
            {
                return result.Rows.Where(x => x.Value.UserID.Equals(userId)).Select(x => new ClientSecretDto()
                {
                    Id = x.Id,
                    Revision = x.Value.Revision,
                    UserID = x.Value.UserID,
                    description = x.Value.description,
                    expiration = x.Value.expiration,
                    SecretId = x.Value.SecretId,
                    type = x.Value.type,
                    value = x.Value.value
                }).ToList();
            }
            else
            {
                return new List<ClientSecretDto>();
            }
        }

        public async Task<bool> RemoveClientApp(string id, string rev)
        {
            if (id == string.Empty)
            {
                return false;
            }

            // Get App Secret ID from couchdb
            var appRes = await couchContext.GetAsync<ClientSecretDto>(couchDbHelper, id).ConfigureAwait(false);
            if (!appRes.IsSuccess)
            {
                return false;
            }
            else
            {
                var res = await apiLao.RemoveClientSecret(appRes.Content.SecretId).ConfigureAwait(false);

                if (res)
                {
                    var result = await couchContext.DeleteAsync(couchDbHelper, id, rev).ConfigureAwait(false);
                    return result.IsSuccess;
                }
            }

            return false;
        }
    }
}
