using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using FreelancerServer.Authentication;

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

        public DbSet<User> Users { get; set; }
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
}
