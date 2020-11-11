using Newtonsoft.Json;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ClientSecretDto : ClientSecret
    {
        public ClientSecretDto(string type, string description, string value, string expired)
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
        [JsonProperty("created")]
        public string Created { get; set; } // Date record created
    }


    public class ListClientSecret
    {
        public int totalCount { get; set; }
        public int pageSize { get; set; }
        public List<Clientsecret> clientSecrets { get; set; }
    }

    public class Clientsecret
    {
        public string type { get; set; }
        public int id { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public DateTime expiration { get; set; }
    }



}
