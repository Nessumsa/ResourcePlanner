﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Interfaces.Managers
{
    public interface IRestApiClient
    {
        Task<bool> ConnectAsync(string host, string port);
        bool DisconnectAsync();
        Task<bool> LoginAsync(string username, string password);
    }
}
