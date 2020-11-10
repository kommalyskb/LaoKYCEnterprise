using Shared.DTOs;
using Shared.Entities;
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
        Task<bool> CreateClientSecret(int? id, ClientSecret secret);
        Task<bool> CreateClientProperty(int? id, ClientPropertyApiDto property);
        Task<bool> CreateClientClaim(int? id, ClientClaimApiDto claim);
        Task<bool> RemoveClientSecret(int? id);
        Task<bool> RemoveClientProperty(int? id);
        Task<bool> RemoveClientClaim(int? id);
        Task<bool> CreateApiResource(ApiResourceApiDto resourceApiDto);
        Task<bool> UpdateResource(ApiResourceApiDto resourceApiDto);
        Task<bool> RemoveResource(int? id);
        Task<bool> CreateResourceSecret(int? id, ApiSecretApiDto secret);
        Task<bool> CreateResourceProperty(int? id, ApiResourcePropertyApiDto property);
        Task<bool> CreateResourceScope(int? id, ApiScopeApiDto scope);
        Task<bool> RemoveResourceSecret(int? id);
        Task<bool> RemoveResourceProperty(int? id);
        Task<bool> RemoveResourceScope(int? id, int? scopeid);
        Task<ClientApiDto> QueryClient(int? id);
        Task<APIResource> QueryAPI(int? id);
        Task<IdentityResourcesDto> QueryIdentityResource(string searchText, int? page, int? pageSize);
    }
}
