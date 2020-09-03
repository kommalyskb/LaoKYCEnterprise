using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class AppClientDto
    {
        public int? Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
    }
}
