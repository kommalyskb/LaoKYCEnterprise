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
    public class IdentityResource : IIdentityResource
    {
        private readonly ICouchContext couchContext;
        private readonly DBConfig dBConfig;
        private readonly IAPILaoKYC apiLao;
        private readonly CouchDBHelper couchDbHelper;
  
        public IdentityResource(ICouchContext couchContext, DBConfig dBConfig, IAPILaoKYC apiLao)
        {
            this.couchContext = couchContext;
            this.dBConfig = dBConfig;
            this.apiLao = apiLao;
            couchDbHelper = new CouchDBHelper(dBConfig.Scheme, dBConfig.SrvAddr,
                "identityresource", dBConfig.Username, dBConfig.Password);
        }

        public async Task<bool> CreateIdentityResource(IdentResource identityResourcesDto)
        {
            if (identityResourcesDto.name == string.Empty)
            {
                return false;
            }

            var result = await couchContext.InsertAsync<IdentResource>(couchDbHelper, identityResourcesDto).ConfigureAwait(false);

            return result.IsSuccess;
        }

        public async Task<List<IdentityResourcesDto>> ListAll()
        {
            var result = await couchContext.ViewQueryAsync<IdentityResourcesDto>(
                    couchDBHelper: couchDbHelper,
                    designName: DesignName.IdentityResourceQuery,
                    viewName: IndexName.IdentityResourceQuery_List,
                    "none",
                    20,
                    0,
                    false,
                    false
                ).ConfigureAwait(false);

            if (result.Rows != null)
            {
                return result.Rows.Select(x => new IdentityResourcesDto()
                {
                    Id = x.Id,
                    Revision = x.Value.Revision,
                    description = x.Value.description,
                    displayName = x.Value.displayName,
                    idrid = x.Value.idrid,
                    name = x.Value.name,
                }).ToList();
            }
            else
            {
                return new List<IdentityResourcesDto>();
            }
        }

        public async Task<bool> RemoveIdentityResource(string id, string rev)
        {
            if (id == string.Empty)
            {
                return false;
            }

            // Get App ID from couchdb
            var appRes = await couchContext.GetAsync<IdentityResourcesDto>(couchDbHelper, id).ConfigureAwait(false);
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

        public async Task<bool> UpdateIdentityResource(IdentityResourcesDto identityResourcesDto)
        {
            if (identityResourcesDto is null)
            {
                return false;
            }

            var result = await couchContext.EditAsync<IdentityResourcesDto>(couchDbHelper, identityResourcesDto).ConfigureAwait(false);
            return result.IsSuccess;
        }
    }
}
