using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models.Database;
using Npgsql;

namespace DataLayer
{
    public class FreelancerContext : DbContext
    {
        public FreelancerContext(DbContextOptions<FreelancerContext> options)
            : base(options)
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<OrderStatus>("order_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<ApplcationStatus>("application_status");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<WorkStatus>("work_status");
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=freelancer_bd;Username=postgres;Password=5690");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Work> Works { get; set; }
    }
}
