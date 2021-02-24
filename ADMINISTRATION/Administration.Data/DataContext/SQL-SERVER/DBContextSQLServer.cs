using Administration.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Data.DataContext.SQL_SERVER
{
    public class DBContextSQLServer : DbContext
    {
        /// <summary>
        /// Отделы.
        /// </summary>
        public DbSet<Department> Departments { get; set; }

        /// <summary></summary>
        /// <param name="options"></param>
        public DBContextSQLServer(DbContextOptions<DBContextSQLServer> options) : base(options)
        {
           // Database.EnsureCreated();
        }

        /// <summary></summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name);
                entity.HasOne(x => x.SubordinateTo)
                    .WithMany(x => x.ManagesDepartments)
                    .HasForeignKey(x => x.SubordinateToId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        
        }

        

    }
}
