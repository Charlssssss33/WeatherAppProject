using ChrnProjectP511.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChrnProjectP511.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<WeatherHistory> History { get; set; }
        public DbSet<WeatherIcon> Icons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {    
            string connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=root";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
