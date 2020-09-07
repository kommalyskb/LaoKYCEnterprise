using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IAPILaoKYC
    {
        Task<bool> CreateClient(ClientApiDto clientApi);
        Task<bool> UpdateClient(ClientApiDto clientApi);
        Task<bool> RemoveClient(int? id);
        Task<bool> CreateClientSecret(int? id, ClientSecretDto secret);
        Task<bool> CreateClientProperty(int? id, ClientPropertyApiDto property);
        Task<bool> CreateClientClaim(int? id, ClientClaimApiDto claim);
        Task<bool> RemoveClientSecret(int? id);
        Task<bool> RemoveClientProperty(int? id);
        Task<bool> RemoveClientClaim(int? id);
    }
}
