using Shared.DTOs;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IMyAppClient
    {
        Task<bool> CreateAppClient(AppClient appClient);
        Task<List<AppClientDto>> ListAll();
        Task<List<AppClientDto>> ListAll(string userId);
        Task<bool> UpdateAppClient(ClientApiDto clientApiDto, AppClientDto appClientDto);
        Task<bool> RemoveClientApp(string id, string rev);
    }
}
