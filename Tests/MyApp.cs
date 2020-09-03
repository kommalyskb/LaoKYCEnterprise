using Portal.Repositories;
using Shared.DTOs;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    
    public class MyApp
    {
        private readonly IMyAppClient myAppClient = new MyAppClient();

        
        [Fact(DisplayName = "ສະແດງລາຍການ Apps(Clients) ທັງຫມົດທີ່ມີ")]
        public async Task ListAllClients()
        {
            var result = await myAppClient.ListAll();

            Assert.NotNull(result);
        }
    }
}
