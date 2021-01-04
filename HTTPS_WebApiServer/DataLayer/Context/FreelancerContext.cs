using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models.Database;
using System;

namespace DataLayer
{
    public class FreelancerContext : DbContext
    {
        public FreelancerContext(DbContextOptions<FreelancerContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=freelancer_bd;Username=postgres;Password=5690");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //translate to string
            modelBuilder
                .Entity<Order>()
                .Property(e => e.Status)
                .HasConversion(
                v => v.ToString(),
                v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));
            modelBuilder
                .Entity<Application>()
                .Property(e => e.Status)
                .HasConversion(
                v => v.ToString(),
                v => (ApplcationStatus)Enum.Parse(typeof(ApplcationStatus), v));
            modelBuilder
                .Entity<Work>()
                .Property(e => e.Status)
                .HasConversion(
                v => v.ToString(),
                v => (WorkStatus)Enum.Parse(typeof(WorkStatus), v));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Work> Works { get; set; }
    }
}
