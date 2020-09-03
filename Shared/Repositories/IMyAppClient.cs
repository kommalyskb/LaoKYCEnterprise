using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface IMyAppClient
    {
        Task<List<AppClientDto>> ListAll();
    }
}
