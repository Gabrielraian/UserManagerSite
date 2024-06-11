using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagerSite.Application.Models;

namespace UserManagerSite.Application.Data
{
    public class UserManagerSiteContext : DbContext
    {
        public UserManagerSiteContext (DbContextOptions<UserManagerSiteContext> options)
            : base(options)
        {
        }

        public DbSet<UserManagerSite.Application.Models.Role> Role { get; set; } = default!;
        public DbSet<UserManagerSite.Application.Models.User> User { get; set; } = default!;
    }
}
