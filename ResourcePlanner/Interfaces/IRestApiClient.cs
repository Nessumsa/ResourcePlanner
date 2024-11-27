using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Interfaces
{
    public interface IRestApiClient
    {
        Task<bool> ConnectAsync(string host, string port);
        Task<bool> DisconnectAsync();
        Task<bool> LoginAsync(string username, string password);
    }
}
