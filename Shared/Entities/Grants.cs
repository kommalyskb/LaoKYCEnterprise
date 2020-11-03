using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class Grants
    {
        [JsonProperty("appid")]
        public string subjectId { get; set; } // This is Subject Id from Identity Server
        [JsonProperty("subjectName")]
        public string subjectName { get; set; } // This is Subject Name from Identity Server
    }
}
