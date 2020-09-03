using Shared.Configs;
using Shared.DTOs;
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
        private readonly CouchDBHelper couchDbHelper;
        private List<AppClientDto> apps;
        
        public MyAppClient(ICouchContext couchContext, DBConfig dBConfig)
        {
            this.couchContext = couchContext;
            this.dBConfig = dBConfig;
        }
        public async Task<List<AppClientDto>> ListAll()
        {
            throw new NotImplementedException();
        }
    }
}
