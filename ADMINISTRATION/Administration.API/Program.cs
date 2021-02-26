using Administration.Data.DataContext.SQL_SERVER;
using Administration.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Administration.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void MigrateDatabase(IServiceCollection services)
        {
            var db = services.BuildServiceProvider().GetService<DBContextSQLServer>();

            Boolean exists = false;

            try
            {
                db.Set<Department>().Count();
                exists = true;
            }
            catch (Exception e)
            {
                exists = false;
            }
            if (!exists)
            {
                db.Database.GetInfrastructure().GetService<IMigrator>().Migrate("Administration");

                try
                {
                    db.Database.ExecuteSqlRaw("insert into Departments (Name, NumberOfStaffUnits, Abolished, CreatedDate, AbolishedDate) values ('-',1, 0, GETDATE(), '20000101')");
                } catch (Exception e)
                {

                }
            }

            

            db.Dispose();

        }
    }
}
