using CouchDBService;
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

            var mocking = new Mock<IAPILaoKYC>();
            
            apiKYC = mocking.Object;
            

            myAppClient = new MyAppClient(couchContext, dBConfig, apiKYC);
        }
        
        [Fact(DisplayName = "ສະແດງລາຍການ Apps(Clients) ທັງຫມົດທີ່ມີ")]
        public async Task ListAllClients()
        {
            var result = await myAppClient.ListAll();

            Assert.NotNull(result);
        }
        [Theory(DisplayName = "ສະແດງລາຍການ Apps(Clients) ທັງຫມົດທີມີ ຕາມ ຂອງ Users ທີ່ເປັນເຈົ້າຂອງ")]
        [InlineData("1qaz2wsx3edc4rfv", 2)]
        [InlineData("", 0)]
        public async Task ListAllClientsBelongtoUser(string userid, int? expected)
        {
            var result = await myAppClient.ListAll(userid);

            Assert.Equal(expected, result.Count);
        }

        [Theory(DisplayName = "ສ້າງ App(Client) ໃຫມ່ ຕາມຂໍ້ມູນເລີ່ມຕົ້ນ ສຳເລັດ")]
        [InlineData("demo_client", "Demo Client App", "This is demo app", "1qaz2wsx", "2020-09-03 19:18:00")]
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

        [Theory(DisplayName = "ສ້າງ App Secrets ຕາມຂໍ້ມູນທີ່ກຳຫນົດ ສຳເລັດ")]
        public async Task CreateClientSecret()
        {

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
                      
                  },
                  AppClientDto = new AppClientDto
                  {
                      AppID = 10,
                      ClientId = "update_client",
                      ClientName = "Update Client App",
                      Description = "This is test update client app",
                      Created = "2020-09-04 00:36:00",
                      Id = "870cbca13cb5e6fb0f6c305643005662",
                      Revision = "2-841561339a4a315501e2e8424b2f04ad",
                      UserID = "1234qwer"
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
