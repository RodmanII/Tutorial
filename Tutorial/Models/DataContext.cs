using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.Utils;

namespace Tutorial.Models
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<IdReceiver> Identifier { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DataContext()
        {

        }
    }
}
