using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ApiResourcePropertyApiDto
    {
        public ApiResourcePropertyApiDto()
        {

        }
        public ApiResourcePropertyApiDto(int id, string key, string value)
        {
            Key = key;
            Value = value;
            Id = id;
        }
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
