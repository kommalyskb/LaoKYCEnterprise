using Shared.DTOs;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IGrantType
    {
        Task<bool> CreateGrantType(GrantTypes grantTypes);
        Task<List<GrantTypeDto>> ListAll();
        Task<bool> UpdateGrantType(GrantTypeDto grantTypeDto);
        Task<bool> RemoveGrantType(string id, string rev);
    }
}
