using Shared.DTOs;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IIdentityResource
    {
        Task<bool> CreateIdentityResource(IdentResource identityResourcesDto);
        Task<List<IdentityResourcesDto>> ListAll();
        Task<bool> UpdateIdentityResource(IdentityResourcesDto identityResourcesDto);
        Task<bool> RemoveIdentityResource(string id, string rev);
    }
}
