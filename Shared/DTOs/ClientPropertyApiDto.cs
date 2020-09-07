using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ClientPropertyApiDto
    {
        public ClientPropertyApiDto(int id, string key, string value)
        {
            this.Id = id;
            this.Key = key;
            this.Value = value;
        }
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
