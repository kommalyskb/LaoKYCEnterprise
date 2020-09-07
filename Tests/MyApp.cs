using CouchDBService;
using IdentityServer4.Models;
using Moq;
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
    
    public class MyApp
    {
        private readonly IAPILaoKYC apiKYC;
        private readonly IMyAppClient myAppClient;
        private ICouchContext couchContext;
        private DBConfig dBConfig;

        public MyApp()
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



            myAppClient = new MyAppClient(couchContext, dBConfig, apiKYC);
        }
        
        [Fact(DisplayName = "ສະແດງລາຍການ Apps(Clients) ທັງຫມົດທີ່ມີ")]
        public async Task ListAllClients()
        {
            var result = await myAppClient.ListAll();

            Assert.NotNull(result);
        }
        [Theory(DisplayName = "ສະແດງລາຍການ Apps(Clients) ທັງຫມົດທີມີ ຕາມ ຂອງ Users ທີ່ເປັນເຈົ້າຂອງ")]
        [InlineData("1qaz2wsx3edc4rfv", 1)]
        [InlineData("", 0)]
        public async Task ListAllClientsBelongtoUser(string userid, int? expected)
        {
            var result = await myAppClient.ListAll(userid);

            Assert.Equal(expected, result.Count);
        }

        [Theory(DisplayName = "ສ້າງ App(Client) ໃຫມ່ ຕາມຂໍ້ມູນເລີ່ມຕົ້ນ ສຳເລັດ")]
        [InlineData("demo_client_333", "Demo Client App", "This is demo app", "qwqe", "2020-09-03 19:18:00")]
        public async Task CreateNewClientApp(string clientid, string clientname, string description,
            string userid, string created)
        {
            AppClient req = new AppClient()
            {
                ClientId = clientid,
                ClientName = clientname,
                Created = created,
                Description = description,
                UserID = userid
            };
            var result = await myAppClient.CreateAppClient(req);

            Assert.True(result);
        }


        [Theory(DisplayName = "ອັບເດດ App(Client) ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [ClassData(typeof(UpdateClientDtoTest))]
        public async Task UpdateClientApp(UpdateClientTestCase param)
        {
            var result = await myAppClient.UpdateAppClient(param.ClientApiDto, param.AppClientDto);

            Assert.True(result);
        }

        [Theory(DisplayName = "ລຶບ App(Client) ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData("870cbca13cb5e6fb0f6c30564301d252", "2-097bd1da4e429d470c31f0ee5468b07e")]
        public async Task RemoveClientApp(string id, string rev)
        {
            
            var result = await myAppClient.RemoveClientApp(id, rev);

            Assert.True(result);
        }

        [Theory(DisplayName = "ສ້າງ App Secrets ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(52, "SharedSecret", "97c67283-f76c-fad5-8db0-f1d2a4f8a7af", "97c67283-f76c-fad5-8db0-f1d2a4f8a7af")]
        public async Task CreateClientSecret(int id, string type, string description, string value)
        {
            string hash = value.Sha256();
            DateTime? expired = DateTime.Now.AddMonths(1);

            var req = new ClientSecretDto(type, description, hash, expired);

            var result = await apiKYC.CreateClientSecret(id, req);

            Assert.True(result);
        }

        [Theory(DisplayName = "ສ້າງ App Properties ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(52, "test", "this is test")]
        public async Task CreateClientProperty(int id, string key, string value)
        {

            var req = new ClientPropertyApiDto(0, key, value);

            var result = await apiKYC.CreateClientProperty(id, req);

            Assert.True(result);
        }

        [Theory(DisplayName = "ສ້າງ App Claims ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(52, "test_claim", "this is test")]
        public async Task CreateClientClaim(int id, string type, string value)
        {

            var req = new ClientClaimApiDto(0, type, value);

            var result = await apiKYC.CreateClientClaim(id, req);

            Assert.True(result);
        }

        [Theory(DisplayName = "ລຶບ App Secrets ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(2)]
        public async Task DeleteClientClaim(int id)
        {
            var result = await apiKYC.RemoveClientClaim(id);

            Assert.True(result);
        }

        [Theory(DisplayName = "ລຶບ App Properties ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        [InlineData(3)]
        public async Task DeleteClientProperty(int id)
        {
            var result = await apiKYC.RemoveClientProperty(id);

            Assert.True(result);
        }


    }
    public class UpdateClientDtoTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new UpdateClientTestCase
                {
                  ClientApiDto = new ClientApiDto
                  {
                      ClientId = "update_client_33",
                      ClientName = "Update Client App",
                      Id = 51
                  },
                  AppClientDto = new AppClientDto
                  {
                      AppID = 51,
                      ClientId = "update_client_33",
                      ClientName = "Update Client App",
                      Description = "This is test update client app",
                      Created = "2020-09-04 00:36:00",
                      Id = "870cbca13cb5e6fb0f6c30564301d252",
                      Revision = "1-2dcbedb146703e4948f46ec08a7890bb",
                      UserID = "qwqe"
                  }
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class UpdateClientTestCase
    {
        public ClientApiDto ClientApiDto { get; set; }
        public AppClientDto AppClientDto { get; set; }
    }
}
