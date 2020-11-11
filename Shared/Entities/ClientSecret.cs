using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class ClientSecret
    {
        public ClientSecret(string type, string description, string value, string expired)
        {
            this.type = type;
            this.value = value;
            this.expiration = expired;
            this.description = description;
        }
        public ClientSecret()
        {

        }
        [JsonProperty("appid")]
        public string AppID { get; set; }
        [JsonProperty("secretid")]
        public int SecretId { get; set; }
        [JsonProperty("type")]
        public string type { get; set; } = "SharedSecret";
        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("value")]
        public string value { get; set; }
        [JsonProperty("hashtype")]
        public string hashtype { get; set; }
        [JsonProperty("expiration")]
        public string expiration { get; set; }
    }
}
