using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Configs
{
    public class CouchDBHelper
    {
        public CouchDBHelper(string scheme, string srvAddr, string dbName, string username, string password, int? port = 5984)
        {
            DbName = dbName;
            ServerAddr = $"{scheme}://{username}:{password}@{srvAddr}:{port}";
        }
        public string ServerAddr { get; set; }
        public string DbName { get; set; }
    }
}
