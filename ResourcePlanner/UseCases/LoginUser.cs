using ResourcePlanner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.UseCases
{
    public class LoginUser
    {
        private readonly IRestApiClient _restApiClient;
        public LoginUser(IRestApiClient restApiClient)
        {
            this._restApiClient = restApiClient;
        }

        public async Task<bool> Execute(string host, string port, string username, string password)
        {
            bool connection = await _restApiClient.ConnectAsync(host, port);
            if (connection)
            {
                bool authenticated = await _restApiClient.LoginAsync(username, password);
                if (authenticated)
                    return true;
            }
            return false;
        }
    }
}
