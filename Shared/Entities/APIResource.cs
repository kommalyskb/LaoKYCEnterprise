using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class APIResource
    {
        [JsonProperty("resid")]
        public int? ResID { get; set; } // This is Resource ID from Identity Server

        [JsonProperty("resname")]
        public string ResName { get; set; } // This is Resource name from Identity Server
        [JsonProperty("display")]
        public string DisplayName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; } // This is Description from Identity Server

        [JsonProperty("userid")]
        public string UserID { get; set; } // UserID who create(sub)

        [JsonProperty("created")]
        public string Created { get; set; } // Date record created
    }
}
