using Shared.DTOs;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IMyAPIResource
    {
        Task<List<APIResourceDto>> ListAll();
        Task<List<APIResourceDto>> ListAll(string userId, int? limit, int? page);
        Task<bool> CreateResource(APIResource req);
        Task<bool> UpdateResource(ApiResourceApiDto apiResourceApiDto, APIResourceDto aPIResource);
        Task<bool> RemoveResource(string id, string rev);
    }
}
