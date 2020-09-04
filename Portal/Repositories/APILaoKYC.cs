using Shared.DTOs;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Repositories
{
    public class APILaoKYC : IAPILaoKYC
    {
        public Task<int> CreateClient(ClientApiDto clientApi)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateClient(ClientApiDto clientApi)
        {
            throw new NotImplementedException();
        }
    }
}
