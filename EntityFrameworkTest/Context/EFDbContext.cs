using EntityFrameworkTest.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTest.Context
{
    public class EFDbContext:DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Work> Works { get; set; }


    }
}
