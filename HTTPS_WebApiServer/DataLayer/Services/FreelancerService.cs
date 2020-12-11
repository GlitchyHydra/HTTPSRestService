using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer.Services
{
    public interface IFreelancerService
    {
        public List<Order> GetOpenedOrders();
        public List<Application> GetApplications();
        public bool InsertApplication(int id);
        public bool MarkAsDone();
    }

    public class FreelancerService : IFreelancerService
    {
        private readonly FreelancerContext context;

        public FreelancerService(FreelancerContext ctx)
        {
            context = ctx;
        }

        public List<Order> GetOpenedOrders()
        {
            return new List<Order>();
        }

        public List<Application> GetApplications()
        {
           return new List<Application>();
        }

        public bool InsertApplication(int id)
        {
            return true;
        }

        public bool MarkAsDone()
        {
            return true;
        }
    }
}
