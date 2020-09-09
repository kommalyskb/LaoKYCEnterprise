using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ApiSecretApiDto
    {
        public ApiSecretApiDto()
        {

        }
        public ApiSecretApiDto(string type, string description, string hash, DateTime? expired)
        {
            Type = type;
            Description = description;
            Value = hash;
            Expiration = expired;
        }
        public string Type { get; set; } = "SharedSecret";

        public int Id { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }

        public DateTime? Expiration { get; set; }
    }
}
