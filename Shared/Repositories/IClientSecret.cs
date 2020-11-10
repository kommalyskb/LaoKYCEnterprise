using Shared.DTOs;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IClientSecret
    {
        Task<bool> CreateClientSecret(ClientSecretDto appClient);
        Task<List<ClientSecretDto>> ListAll();
        Task<List<ClientSecretDto>> ListAll(int? AppID, string userId, int? limit, int? page);
        Task<bool> RemoveClientApp(string id, string rev);
    }
}
