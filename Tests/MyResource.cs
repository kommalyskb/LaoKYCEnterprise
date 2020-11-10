using CouchDBService;
using IdentityServer4.Models;
using Portal.Repositories;
using Shared.Configs;
using Shared.DTOs;
using Shared.Entities;
using Shared.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class MyResource
    {
        private IAPILaoKYC apiKYC;
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
            var result = await myAPIResouce.ListAll().ConfigureAwait(false);

            Assert.NotNull(result);
        }
        [Theory(DisplayName = "ສະແດງລາຍການ API(API Resources) ທັງຫມົດທີມີ ຕາມ ຂອງ Users ທີ່ເປັນເຈົ້າຂອງ")]
        [InlineData("1qaz2wsx", 20, 0)]
        public async Task ListAllResourceBelongtoUser(string userid, int? limit, int? page)
        {
            var result = await myAPIResouce.ListAll(userid, limit, page).ConfigureAwait(false);

            Assert.NotNull(result);
        }

        [Theory(DisplayName = "ສ້າງ API(API Resources) ໃຫມ່ ຕາມຂໍ້ມູນເລີ່ມຕົ້ນ ສຳເລັດ")]
        [InlineData("DemoResource", "Demo Api Resource", "This is demo resource", "qwqe", "2020-09-03 19:18:00")]
        public async Task CreateNewResource(string apiname, string display, string description,
            string userid, string created)
        {
            APIResource req = new APIResource()
            {
                ResName = apiname,
                Created = created,
                Description = description,
                UserID = userid,
                DisplayName = display
            };
            var result = await myAPIResouce.CreateResource(req).ConfigureAwait(false);

            Assert.True(result);
        }


        [Theory(DisplayName = "ອັບເດດ API(API Resources) ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [ClassData(typeof(UpdateResourceDtoTest))]
        public async Task UpdateResource(UpdateResourceTestCase param)
        {
            var result = await myAPIResouce.UpdateResource(param.apiResourceApiDto, param.aPIResource).ConfigureAwait(false);

            Assert.True(result);
        }

        [Theory(DisplayName = "ລຶບ API(API Resources) ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData("aeef23159752bdf7f0dbc1a5ab002a22", "2-a2f4fcb839c37e379ebcaabda3a98958")]
        public async Task RemoveResource(string id, string rev)
        {

            var result = await myAPIResouce.RemoveResource(id, rev).ConfigureAwait(false);

            Assert.True(result);
        }

        [Theory(DisplayName = "ສ້າງ API Secrets ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(7, "SharedSecret", "9f0cd207-0d3c-71c7-73be-1a107c785e83", "9f0cd207-0d3c-71c7-73be-1a107c785e83")]
        public async Task CreateResourceSecret(int id, string type, string description, string value)
        {
            string hash = value.Sha256();
            DateTime? expired = DateTime.Now.AddMonths(1);

            var req = new ApiSecretApiDto(type, description, hash, expired);

            var result = await apiKYC.CreateResourceSecret(id, req).ConfigureAwait(false);

            Assert.True(result);
        }

        [Theory(DisplayName = "ສ້າງ API Properties ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(7, "test", "this is test")]
        public async Task CreateResourceProperty(int id, string key, string value)
        {

            var req = new ApiResourcePropertyApiDto(0, key, value);

            var result = await apiKYC.CreateResourceProperty(id, req).ConfigureAwait(false);

            Assert.True(result);
        }

        [Theory(DisplayName = "ສ້າງ API Scope ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(7, "scope1", "this is scope 1", "this is scope 1")]
        public async Task CreateResourceScope(int id, string name, string display, string description)
        {
            var userClaims = new List<string>()
            {
                "claim1",
                "claim2",
                "claim3"
            };
            var req = new ApiScopeApiDto(0, name, display, description, true, true, true, userClaims);

            var result = await apiKYC.CreateResourceScope(id, req).ConfigureAwait(false);

            Assert.True(result);
        }

        [Theory(DisplayName = "ລຶບ API Secret ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(2)]
        public async Task DeleteResourceSecret(int id)
        {
            var result = await apiKYC.RemoveResourceSecret(id).ConfigureAwait(false);

            Assert.True(result);
        }

        [Theory(DisplayName = "ລຶບ API Properties ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(1)]
        public async Task DeleteResourceProperty(int id)
        {
            var result = await apiKYC.RemoveResourceProperty(id).ConfigureAwait(false);

            Assert.True(result);
        }

        [Theory(DisplayName = "ລຶບ API Scope ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(7,3)]
        public async Task DeleteResourceScope(int id, int scopeid)
        {
            var result = await apiKYC.RemoveResourceScope(id, scopeid).ConfigureAwait(false);

            Assert.True(result);
        }

    }

    public class UpdateResourceDtoTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new UpdateResourceTestCase
                {
                  apiResourceApiDto = new ApiResourceApiDto
                  {
                      Id = 6,
                      DisplayName = "abohabe",
                      Description = "This is test update resource",
                      Enabled = true,
                      Name = "Poiuytl"
                  },
                  aPIResource = new APIResourceDto
                  {
                      DisplayName = "abohabe",
                      ResID = 6,
                      ResName = "Poiuytl",
                      Description = "This is test update resource",
                      Created = "2020-09-04 00:36:00",
                      Id = "aeef23159752bdf7f0dbc1a5ab002a22",
                      Revision = "1-30ec63c6058b2921c70bf923718fd474",
                      UserID = "qwqe"
                  }
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class UpdateResourceTestCase
    {
        public ApiResourceApiDto apiResourceApiDto { get; set; }
        public APIResourceDto aPIResource { get; set; }
    }
}
