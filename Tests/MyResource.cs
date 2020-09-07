using CouchDBService;
using Portal.Repositories;
using Shared.Configs;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class MyResource
    {
        private APILaoKYC apiKYC;
        private IMyAPIResource myAPIResouce;
        private ICouchContext couchContext;
        private DBConfig dBConfig;

        public MyResource()
        {
            this.couchContext = new CouchContext();
            this.dBConfig = new DBConfig()
            {
                Password = "1qaz2wsx",
                Port = 5984,
                Scheme = "http",
                SrvAddr = "127.0.0.1",
                Username = "admin"
            };


            apiKYC = new APILaoKYC();



            myAPIResouce = new MyAPIResource(couchContext, dBConfig, apiKYC);
        }
        [Fact(DisplayName = "ສະແດງລາຍການ API(API Resources) ທັງຫມົດທີ່ມີ")]
        public async Task ListAllResource()
        {
            var result = await myAPIResouce.ListAll();

            Assert.NotNull(result);
        }
    }
}
