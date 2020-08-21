using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class JmvgDbContext:DbContext
    {
        private static string username;
        private static string password;

        public static void Initialize(string username, string password)
        {
            JmvgDbContext.username = username;
            JmvgDbContext.password = password;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = new SqlConnectionStringBuilder
            {
                DataSource = "jmvg.database.windows.net",
                Authentication = SqlAuthenticationMethod.SqlPassword,
                InitialCatalog = "jmvg",
                UserID = username,
                Password = password
            }.ConnectionString;

            optionsBuilder.UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public DbSet<Video> Videos { get; set; }

        public DbSet<Instance> Instances { get; set; }
    }
}
