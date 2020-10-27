using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shared.Entities;

namespace Shared.DTOs
{
    public class AppClientDto: AppClient
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("_rev")]
        public string Revision { get; set; }
    }
}
