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
    public class MyGrants
    {
        private IAPILaoKYC apiKYC;
        private IMyGrant myGrant;
        private ICouchContext couchContext;
        private DBConfig dBConfig;

        public MyGrants()
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
            myGrant = new MyGrant(couchContext, dBConfig, apiKYC);
        }

        [Fact(DisplayName = "ສະແດງລາຍການ Apps(grant) ທັງຫມົດທີ່ມີ")]
        public async Task ListAllClients()
        {
            var result = await myGrant.ListAll();

            Assert.NotNull(result);
        }

        [Theory(DisplayName = "ສະແດງລາຍການ Apps(grant) ທັງຫມົດທີມີ ຕາມ ຂອງ Users ທີ່ເປັນເຈົ້າຂອງ")]
        [InlineData("1qaz2wsx3edc4rfv", 1, 20, 0)]
        [InlineData("", 0, 20, 0)]
        public async Task ListAllClientsBelongtoUser(string userid, int? expected, int? limit, int? page)
        {
            var result = await myGrant.ListAll(userid, limit, page);
            Assert.Equal(expected, result.Count);
        }

        [Theory(DisplayName = "ລຶບ App(grant) ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData("870cbca13cb5e6fb0f6c30564301d252", "2-097bd1da4e429d470c31f0ee5468b07e")]
        public async Task RemoveClientApp(string id, string rev)
        {

            var result = await myGrant.RemoveGrant(id, rev);

            Assert.True(result);
        }

    }
}
