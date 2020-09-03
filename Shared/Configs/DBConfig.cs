using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Configs
{
    public class DBConfig
    {
        public string Scheme { get; set; }
        public string SrvAddr { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? Port { get; set; }
    }
}
