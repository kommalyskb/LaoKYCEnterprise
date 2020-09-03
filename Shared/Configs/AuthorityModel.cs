using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Configs
{
    public class AuthorityModel
    {
        public string Debug { get; set; }
        public string Release { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public bool SaveTokens { get; set; }
        public string SignInScheme { get; set; }
        public string ResponseType { get; set; }
        public bool GetClaimsFromUserInfoEndpoint { get; set; }
    }
}
