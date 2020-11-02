using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Configs
{
    public static class IdentityEndpoint
    {
        public static string Discovery = "https://login.oneid.sbg.la";
        public static string ClientID = "AdminClient_api_swaggerui";
        public static string Secret = "b7115a75-54eb-91d4-3736-78364e002fc2";
        public static string Scopes = "AdminClient_api";
        public static string UserName = "admin";
        public static string Password = "@Admin1qaz2wsx@Cz";
        public static string ClientUri = "https://api.oneid.sbg.la/api/Clients";
        public static string ResourceUri = "https://api.oneid.sbg.la/api/ApiResources";
        public static string GrantUri = "https://api.oneid.sbg.la/api/PersistedGrants";
    }
}
