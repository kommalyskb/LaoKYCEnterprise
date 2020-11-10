using CouchDBService;
using Portal.Repositories;
using Shared.Configs;
using Shared.DTOs;
using Shared.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class IdentityResource
    {
        private readonly IAPILaoKYC apiKYC;
        private readonly IIdentityResource identityResource;
        private ICouchContext couchContext;
        private DBConfig dBConfig;

        public IdentityResource()
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
            identityResource = new Portal.Repositories.IdentityResource(couchContext, dBConfig, apiKYC);
        }

        [Fact(DisplayName = "ສະແດງລາຍການ Identity Resource ທັງຫມົດທີ່ມີ")]
        public async Task ListAllIdentityRes()
        {
            var result = await identityResource.ListAll().ConfigureAwait(false);

            Assert.NotNull(result);
        }

        [Theory(DisplayName = "ສ້າງ identity Resource ໃຫມ່ ຕາມຂໍ້ມູນເລີ່ມຕົ້ນ ສຳເລັດ")]
        [InlineData("IdentityResource", "Demo Identity Resource", "This is Identity Resource", "qwqe", "2020-09-03 19:18:00")]
        public async Task CreateNewIdentityRes(string name, string display, string description, string userid, string created)
        {
            Shared.Entities.IdentResource req = new Shared.Entities.IdentResource()
            {
                name = name,
                displayName = display,
                description = description,
                UserID = userid,
                Created = created
            };
            var result = await identityResource.CreateIdentityResource(req).ConfigureAwait(false);

            Assert.True(result);
        }

        [Theory(DisplayName = "ອັບເດດ identity Resource ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [ClassData(typeof(IdentResourceTest))]
        public async Task UpdateResource(IdentityResourcesDto param)
        {
            var result = await identityResource.UpdateIdentityResource(param).ConfigureAwait(false);

            Assert.True(result);
        }

        [Theory(DisplayName = "ລຶບ identity Resource ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData("870cbca13cb5e6fb0f6c30564301d252", "2-097bd1da4e429d470c31f0ee5468b07e")]
        public async Task RemoveClientApp(string id, string rev)
        {

            var result = await identityResource.RemoveIdentityResource(id, rev).ConfigureAwait(false);

            Assert.True(result);
        }

    }

    public class IdentResourceTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new IdentityResourcesDto
                {
                    Created = "2020-09-03 19:18:00",
                    description = "this is test update identity resouce",
                    displayName = "display name",
                    Id = "aeef23159752bdf7f0dbc1a5ab002a22",
                    idrid = 6,
                    name = "IdentityResource",
                    Revision = "1-30ec63c6058b2921c70bf923718fd474",
                    UserID = "qwqe"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
