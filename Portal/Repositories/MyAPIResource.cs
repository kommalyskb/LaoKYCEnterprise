﻿using CouchDBService;
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

        public async Task<bool> CreateResource(APIResource req)
        {
            if (req.ResName == string.Empty)
            {
                return false;
            }

            var apires = new ApiResourceApiDto()
            {
                Description = req.Description,
                DisplayName = req.ResName,
                Enabled = true,
                Name = req.ResName
            };
            var res = await apiLao.CreateApiResource(apires);
            if (res)
            {
                req.ResID = apires.Id;
                var result = await couchContext.InsertAsync<APIResource>(couchDbHelper, req);

                return result.IsSuccess;
            }

            return false;
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
            if (result.Rows != null)
            {
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
            else
            {
                return new List<APIResourceDto>();
            }
        }

        public async Task<List<APIResourceDto>> ListAll(string userId, int? limit, int? page)
        {
            int skip = limit.Value * page.Value;
            var result = await couchContext.ViewQueryAsync<APIResourceDto>(
                    couchDBHelper: couchDbHelper,
                    designName: DesignName.ApiResourceQuery,
                    viewName: IndexName.ApiResourceQuery_List,
                    userId,
                    limit.Value,
                    skip,
                    false,
                    false
                );
            if (result.Rows != null)
            {
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
            else
            {
                return new List<APIResourceDto>();
            }
            
        }

        public async Task<bool> RemoveResource(string id, string rev)
        {
            if (id == string.Empty)
            {
                return false;
            }

            // Get App ID from couchdb
            var appRes = await couchContext.GetAsync<APIResource>(couchDbHelper, id);
            if (!appRes.IsSuccess)
            {
                return false;
            }

            var res = await apiLao.RemoveResource(appRes.Content.ResID);

            if (res)
            {
                var result = await couchContext.DeleteAsync(couchDbHelper, id, rev);
                return result.IsSuccess;
            }
            return false;
        }

        public async Task<bool> UpdateResource(ApiResourceApiDto apiResourceApiDto, APIResourceDto aPIResource)
        {
            if (aPIResource.Id == string.Empty)
            {
                return false;
            }
            if (aPIResource.Revision == string.Empty)
            {
                return false;
            }

            var res = await apiLao.UpdateResource(apiResourceApiDto);

            if (res)
            {
                var result = await couchContext.EditAsync<APIResourceDto>(couchDbHelper, aPIResource);
                return result.IsSuccess;
            }
            return false;
        }
    }
}
