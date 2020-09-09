using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ApiResourceApiDto
    {
        public ApiResourceApiDto()
        {
            UserClaims = new List<string>();
        }
        public ApiResourceApiDto(int id, string name, string display, string description,
            bool enable, List<string> userClaims)
        {
            Id = id;
            Name = name;
            DisplayName = display;
            Description = description;
            Enabled = enable;
            UserClaims = userClaims;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; } = true;

        public List<string> UserClaims { get; set; }
    }
}
