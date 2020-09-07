using CouchDBService;
using Shared.Configs;
using Shared.DTOs;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Repositories
{
    public class MyAPIResource: IMyAPIResource
    {
        private readonly ICouchContext couchContext;
        private readonly DBConfig dBConfig;
        private readonly IAPILaoKYC apiLao;
        private readonly CouchDBHelper couchDbHelper;

        public MyAPIResource(ICouchContext couchContext, DBConfig dBConfig, IAPILaoKYC apiLao)
        {
            this.couchContext = couchContext;
            this.dBConfig = dBConfig;
            this.apiLao = apiLao;
            couchDbHelper = new CouchDBHelper(dBConfig.Scheme, dBConfig.SrvAddr,
                "apiresource", dBConfig.Username, dBConfig.Password);
        }

        public async Task<List<APIResourceDto>> ListAll()
        {
            var result = await couchContext.ViewQueryAsync<APIResourceDto>(
                    couchDBHelper: couchDbHelper,
                    designName: DesignName.ApiResourceQuery,
                    viewName: IndexName.ApiResourceQuery_List,
                    "none",
                    20,
                    0,
                    false,
                    false
                );
            return result.Rows.Select(x => new APIResourceDto()
            {
                ResID = x.Value.ResID,
                ResName = x.Value.ResName,
                Created = x.Value.Created,
                Description = x.Value.Description,
                Id = x.Id,
                Revision = x.Value.Revision,
                UserID = x.Value.UserID
            }).ToList();
        }

        public Task<List<APIResourceDto>> ListAll(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
