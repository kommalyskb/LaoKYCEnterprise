using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class IdentResource
    {
        [JsonProperty("idrid")]
        public int idrid { get; set; }  // This is Ident Resource ID from Identity Server
        [JsonProperty("name")] 
        public string name { get; set; } // This is Ident Resource name from Identity Server
        [JsonProperty("displayName")] 
        public string displayName { get; set; }  // This is Ident Resource display from Identity Server
        [JsonProperty("description")]
        public string description { get; set; }  // This is Ident Resource description from Identity Server

        [JsonProperty("userid")]
        public string UserID { get; set; } // UserID who create(sub)

        [JsonProperty("created")]
        public string Created { get; set; } // Date record created
    }
}
