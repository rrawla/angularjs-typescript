using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Data;

namespace Infrastructure.Data
{
    public class Team
    {
        public long TeamId { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Division { get; set; }
    }
}