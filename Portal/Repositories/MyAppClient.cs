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
    public class MyAppClient : IMyAppClient
    {
        private readonly ICouchContext couchContext;
        private readonly DBConfig dBConfig;
        private readonly IAPILaoKYC apiLao;
        private readonly CouchDBHelper couchDbHelper;
        private List<AppClientDto> apps;

        public MyAppClient(ICouchContext couchContext, DBConfig dBConfig, IAPILaoKYC apiLao)
        {
            this.couchContext = couchContext;
            this.dBConfig = dBConfig;
            this.apiLao = apiLao;
            couchDbHelper = new CouchDBHelper(dBConfig.Scheme, dBConfig.SrvAddr,
                "appclient", dBConfig.Username, dBConfig.Password);
        }
        public async Task<List<AppClientDto>> ListAll()
        {
            var result = await couchContext.ViewQueryAsync<AppClientDto>(
                    couchDBHelper: couchDbHelper,
                    designName: DesignName.AppClientQuery,
                    viewName: IndexName.AppClientQuery_List,
                    "none",
                    20,
                    0,
                    false,
                    false
                );
            return result.Rows.Select(x => new AppClientDto()
            {
                AppID = x.Value.AppID,
                ClientId = x.Value.ClientId,
                ClientName = x.Value.ClientName,
                Created = x.Value.Created,
                Description = x.Value.Description,
                Id = x.Id,
                Revision = x.Value.Revision,
                UserID = x.Value.UserID
            }).ToList();
        }
        public async Task<List<AppClientDto>> ListAll(string userId)
        {
            if (userId == string.Empty)
            {
                return await Task.FromResult(new List<AppClientDto>());
            }

            var result = await couchContext.ViewQueryAsync<AppClientDto>(
                    couchDBHelper: couchDbHelper,
                    designName: DesignName.AppClientQuery,
                    viewName: IndexName.AppClientQuery_List,
                    userId,
                    20,
                    0,
                    false,
                    false
                );
            return result.Rows.Select(x => new AppClientDto()
            {
                AppID = x.Value.AppID,
                ClientId = x.Value.ClientId,
                ClientName = x.Value.ClientName,
                Created = x.Value.Created,
                Description = x.Value.Description,
                Id = x.Id,
                Revision = x.Value.Revision,
                UserID = x.Value.UserID
            }).ToList();
        }

        public async Task<bool> CreateAppClient(AppClient appClient)
        {
            if (appClient.ClientId == string.Empty)
            {
                return await Task.FromResult(false);
            }
            if (appClient.UserID == string.Empty)
            {
                return await Task.FromResult(false);
            }

            var id = await apiLao.CreateClient(new ClientApiDto());

            appClient.AppID = id;

            var result = await couchContext.InsertAsync<AppClient>(couchDbHelper, appClient);

            return result.IsSuccess;
        }

        public async Task<bool> UpdateAppClient(ClientApiDto clientApiDto, AppClientDto appClientDto)
        {
            if (clientApiDto is null)
            {
                return await Task.FromResult(false);
            }
            if (appClientDto is null)
            {
                return await Task.FromResult(false);
            }
            if (appClientDto.Id == string.Empty)
            {
                return await Task.FromResult(false);
            }
            if (appClientDto.Revision == string.Empty)
            {
                return await Task.FromResult(false);
            }

            var res = await apiLao.UpdateClient(clientApiDto);

            //if (res)
            //{
            //    var result = await couchContext.EditAsync<AppClientDto>(couchDbHelper, appClientDto);
            //    return result.IsSuccess;
            //}
            //return false;
            var result = await couchContext.EditAsync<AppClientDto>(couchDbHelper, appClientDto);
            return result.IsSuccess;
        }
    }
}
