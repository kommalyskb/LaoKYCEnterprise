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
    public class MyGrant : IMyGrant
    {
        private readonly ICouchContext couchContext;
        private readonly DBConfig dBConfig;
        private readonly IAPILaoKYC apiLao;
        private readonly CouchDBHelper couchDbHelper;
      
        public MyGrant(ICouchContext couchContext, DBConfig dBConfig, IAPILaoKYC apiLao)
        {
            this.couchContext = couchContext;
            this.dBConfig = dBConfig;
            this.apiLao = apiLao;
            couchDbHelper = new CouchDBHelper(dBConfig.Scheme, dBConfig.SrvAddr,
                "appgrant", dBConfig.Username, dBConfig.Password);
        }

        public async Task<List<GrantsDto>> ListAll()
        {
            var result = await couchContext.ViewQueryAsync<GrantsDto>(
                   couchDBHelper: couchDbHelper,
                   designName: DesignName.AppGrantQuery,
                   viewName: IndexName.AppGrantQuery_List,
                   "none",
                   20,
                   0,
                   false,
                   false
               );

            if (result.Rows != null)
            {
                return result.Rows.Select(x => new GrantsDto()
                {
                    Id = x.Id,
                    Revision = x.Value.Revision,
                    subjectId = x.Value.subjectId,
                    subjectName = x.Value.subjectName
                }).ToList();
            }
            else
            {
                return new List<GrantsDto>();
            }
        }

        public async Task<List<GrantsDto>> ListAll(string userId, int? limit, int? page)
        {
            if (userId == string.Empty)
            {
                return await Task.FromResult(new List<GrantsDto>());
            }

            int skip = limit.Value * page.Value;
            var result = await couchContext.ViewQueryAsync<GrantsDto>(
                    couchDBHelper: couchDbHelper,
                    designName: DesignName.AppGrantQuery,
                    viewName: IndexName.AppGrantQuery_List,
                    userId,
                    limit.Value,
                    skip,// skip = page * limit
                    false,
                    false
                );
            if (result.Rows != null)
            {
                return result.Rows.Select(x => new GrantsDto()
                {
                    Id = x.Id,
                    Revision = x.Value.Revision,
                    subjectId = x.Value.subjectId,
                    subjectName = x.Value.subjectName
                }).ToList();
            }
            else
            {
                return new List<GrantsDto>();
            }
        }

        public async Task<bool> RemoveGrant(string id, string rev)
        {
            if (id == string.Empty)
            {
                return false;
            }

            // Get App ID from couchdb
            var appRes = await couchContext.GetAsync<GrantsDto>(couchDbHelper, id);
            System.Threading.Thread.Sleep(500);
            if (!appRes.IsSuccess)
            {
                return false;
            }
            else
            {
                var result = await couchContext.DeleteAsync(couchDbHelper, id, rev);
                return result.IsSuccess;
            }

        }
    }
}
