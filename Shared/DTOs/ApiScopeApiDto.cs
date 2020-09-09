using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class ApiScopeApiDto
    {
        public ApiScopeApiDto()
        {
            UserClaims = new List<string>();
        }
        public ApiScopeApiDto(int id, string name, string display, string description, 
            bool required, bool emphasize, bool showInDiscovery, List<string> userClaims)
        {
            Id = id;
            Name = name;
            Description = description;
            Required = required;
            Emphasize = emphasize;
            ShowInDiscoveryDocument = showInDiscovery;
            UserClaims = userClaims;
        }

        public bool ShowInDiscoveryDocument { get; set; } = true;

        public int Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public List<string> UserClaims { get; set; }
    }
}
