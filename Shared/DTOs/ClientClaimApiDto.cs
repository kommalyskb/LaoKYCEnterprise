using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ClientClaimApiDto
    {
        public ClientClaimApiDto(int id, string type, string value)
        {
            this.Id = id;
            this.Type = type;
            this.Value = value;
        }
        public ClientClaimApiDto()
        {

        }
        public int Id { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
