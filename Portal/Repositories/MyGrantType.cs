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
    public class MyGrantType : IGrantType
    {
        private readonly ICouchContext couchContext;
        private readonly DBConfig dBConfig;
        private readonly IAPILaoKYC apiLao;
        private readonly CouchDBHelper couchDbHelper;

        public MyGrantType(ICouchContext couchContext, DBConfig dBConfig, IAPILaoKYC apiLao)
        {
            this.couchContext = couchContext;
            this.dBConfig = dBConfig;
            this.apiLao = apiLao;
            couchDbHelper = new CouchDBHelper(dBConfig.Scheme, dBConfig.SrvAddr,
                "granttype", dBConfig.Username, dBConfig.Password);
        }

        public async Task<bool> CreateGrantType(GrantTypes grantTypes)
        {
            if (grantTypes.Value == string.Empty)
            {
                return false;
            }

            var result = await couchContext.InsertAsync<GrantTypes>(couchDbHelper, grantTypes).ConfigureAwait(false);

            return result.IsSuccess;
        }

        public async Task<List<GrantTypeDto>> ListAll()
        {
            var result = await couchContext.ViewQueryAsync<GrantTypeDto>(
                    couchDBHelper: couchDbHelper,
                    designName: DesignName.GrantTypeQuery,
                    viewName: IndexName.GrantTypeQuery_List,
                    "none",
                    20,
                    0,
                    false,
                    false
                ).ConfigureAwait(false);

            if (result.Rows != null)
            {
                return result.Rows.Select(x => new GrantTypeDto()
                {
                    Id = x.Id,
                    Revision = x.Value.Revision,
                    Text = x.Value.Text,
                    Value = x.Value.Value
                }).ToList();
            }
            else
            {
                return new List<GrantTypeDto>();
            }
        }

        public async Task<bool> RemoveGrantType(string id, string rev)
        {
            if (id == string.Empty)
            {
                return false;
            }

            // Get App ID from couchdb
            var appRes = await couchContext.GetAsync<GrantTypeDto>(couchDbHelper, id).ConfigureAwait(false);
            System.Threading.Thread.Sleep(500);
            if (!appRes.IsSuccess)
            {
                return false;
            }
            else
            {
                var result = await couchContext.DeleteAsync(couchDbHelper, id, rev).ConfigureAwait(false);
                return result.IsSuccess;
            }
        }

        public async Task<bool> UpdateGrantType(GrantTypeDto grantTypeDto)
        {
            if (grantTypeDto is null)
            {
                return false;
            }

            var result = await couchContext.EditAsync<GrantTypeDto>(couchDbHelper, grantTypeDto).ConfigureAwait(false);
            return result.IsSuccess;
        }
    }
}
