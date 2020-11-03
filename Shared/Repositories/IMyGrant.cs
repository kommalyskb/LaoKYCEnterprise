using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IMyGrant
    {
        Task<List<GrantsDto>> ListAll();
        Task<List<GrantsDto>> ListAll(string userId, int? limit, int? page);
        Task<bool> RemoveGrant(string id, string rev);
    }
}
