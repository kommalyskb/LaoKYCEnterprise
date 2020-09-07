using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ClientSecretDto
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
        public string type { get; set; }
        public string description { get; set; }
        public string value { get; set; }
        public DateTime? expiration { get; set; }
    }

}
