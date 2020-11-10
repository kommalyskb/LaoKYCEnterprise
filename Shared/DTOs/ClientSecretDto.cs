using Newtonsoft.Json;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ClientSecretDto : ClientSecret
    {
        public ClientSecretDto(string type, string description, string value, DateTime? expired)
        {
            this.type = type;
            this.value = value;
            this.expiration = expired;
            this.description = description;
        }
        public ClientSecretDto()
        {

        }

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("userid")]
        public string UserID { get; set; }
        [JsonProperty("_rev")]
        public string Revision { get; set; }
    }

}
