using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IAPILaoKYC
    {
        Task<int> CreateClient(ClientApiDto clientApi);
        Task<bool> UpdateClient(ClientApiDto clientApi);
    }
}
