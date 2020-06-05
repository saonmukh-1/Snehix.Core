using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Snehix.Core.API.Entities
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConnectionString());
        }
        
        private static string GetConnectionString()
        {
            const string databaseName = "blackboard";
            const string databaseUser = "admin";
            const string databasePass = "welcome2020";
            //return "server=database-1.ctxnhfdfmdii.ap-south-1.rds.amazonaws.com;user=admin;password=welcome2020;database=blackboard";
            return $"server=database-1.ctxnhfdfmdii.ap-south-1.rds.amazonaws.com;" +
                   $"database={databaseName};" +
                   $"uid={databaseUser};" +
                   $"pwd={databasePass};" +
                   $"pooling=true;";
        }
    }
}