using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Web.Configuration;

namespace STO_Man_WcfService.Model
{
    public class StoDbContext : DbContext
    {
        public DbSet<Station> Stations { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServedCar> ServedCars { get; set; }

        public StoDbContext()
            : base()
        {
        }

        //public StoDbContext(DbContextOptions<StoDbContext> options)
        //    : base(options)
        //{
        //    Database.EnsureDeleted();
        //    Database.EnsureCreated();
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["AppDatabaseConnectionString"].ConnectionString;
            optionsBuilder.UseNpgsql(connectionString);
            //optionsBuilder.UseNpgsql("Server=localhost;Port=5000;User Id=postgres;Password=saPg123456;Database=STO_DB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Определение внешних ключей посредством FluentApi
            modelBuilder.Entity<Service>()
                .HasOne(p => p.Station)
                .WithMany(t => t.Services)
                .HasForeignKey(p => p.StationId);

            modelBuilder.Entity<ServedCar>()
                .HasOne(p => p.Service)
                .WithMany(t => t.ServedCars)
                .HasForeignKey(p => p.ServiceId);
        }
    }
}