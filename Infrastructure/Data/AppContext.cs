using System;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Infrastructure;

namespace Infrastructure.Data
{
    public class AppContext : DbContext
    {
        public AppContext() : base("name=DefaultConnection")
        {

        }
        public DbSet<Setting> Settings { get; set; }
    }
}
