using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientToMyServer.Services
{
    public interface IHttpCodeWriter
    {
        Task<string> GetCodeById(int id);
        Task Authentificate();
    }
}
