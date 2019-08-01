using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SkinAppBackend.Storage
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) {
            this.Database.SetCommandTimeout(0);
        }

        public virtual DbSet<Models.User> Users { get; set; }
        public virtual DbSet<Models.Section> Sections { get; set; }
        public virtual DbSet<Models.Picture> Pictures { get; set; }

    }
}
