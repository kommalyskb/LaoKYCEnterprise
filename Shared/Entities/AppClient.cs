using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Shared.Entities
{
    public class AppClient
    {
        [JsonProperty("appid")]
        public int? AppID { get; set; } // This is Id from Identity Server

        [JsonProperty("clientid")] 
        public string ClientId { get; set; } // This is ClientID from Identity Server

        [JsonProperty("clientname")]
        public string ClientName { get; set; } // This is Client name from Identity Server

        [JsonProperty("description")]
        public string Description { get; set; } // This is Description from Identity Server

        [JsonProperty("userid")]
        public string UserID { get; set; } // UserID who create(sub)

        [JsonProperty("created")]
        public string Created { get; set; } // Date record created
    }
}
