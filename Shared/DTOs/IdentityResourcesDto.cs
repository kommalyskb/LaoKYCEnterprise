﻿using Newtonsoft.Json;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class IdentityResourcesDto : IdentResource
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("_rev")]
        public string Revision { get; set; }
    }

}
