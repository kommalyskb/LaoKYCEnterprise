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
    public class MyGrantTypes
    {
        private IAPILaoKYC apiKYC;
        private IGrantType grantType;
        private ICouchContext couchContext;
        private DBConfig dBConfig;

        public MyGrantTypes()
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
            grantType = new MyGrantType(couchContext, dBConfig, apiKYC);
        }

        [Fact(DisplayName = "ສະແດງລາຍການ Grant type ທັງຫມົດທີ່ມີ")]
        public async Task ListAllGrantType()
        {
            var result = await grantType.ListAll();

            Assert.NotNull(result);
        }

        [Theory(DisplayName = "ສ້າງ Grant type ໃຫມ່ ຕາມຂໍ້ມູນເລີ່ມຕົ້ນ ສຳເລັດ")]
        [InlineData("hybrid", "hybrid")]
        public async Task CreateNewIdentityRes(string value, string text)
        {
            Shared.Entities.GrantTypes req = new Shared.Entities.GrantTypes()
            {
                Value = value,
                Text = text
            };
            var result = await grantType.CreateGrantType(req);

            Assert.True(result);
        }

        [Theory(DisplayName = "ອັບເດດ Grant type ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [ClassData(typeof(GrantTypeDtoTest))]
        public async Task UpdateResource(GrantTypeDto param)
        {
            var result = await grantType.UpdateGrantType(param);

            Assert.True(result);
        }

        [Theory(DisplayName = "ລຶບ Grant type ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData("870cbca13cb5e6fb0f6c30564301d252", "2-097bd1da4e429d470c31f0ee5468b07e")]
        public async Task RemoveClientApp(string id, string rev)
        {

            var result = await grantType.RemoveGrantType(id, rev);

            Assert.True(result);
        }


        public class GrantTypeDtoTest : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
               {
                     new GrantTypeDto
                    {
                        Id = "aeef23159752bdf7f0dbc1a5ab002a22",
                        Revision = "1-30ec63c6058b2921c70bf923718fd474",
                        Text = "hybrid",
                        Value = "hybrid"
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


    }

}
