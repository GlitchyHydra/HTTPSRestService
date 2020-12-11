using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Services
{
    public interface ICustomerService
    {
        public Task<bool> InsertOrder(string name, string description);
        public Task<List<Order>> GetOrders();
        public Task<bool> AlterOrderStatus(int id, OrderStatus status);
        public Task<bool> AcceptApplyFromFreelancer(int application_id);
        public Task<bool> AcceptCompletedWork();
    }

    public class CustomerService : ICustomerService
    {
        private readonly FreelancerContext context;

        public CustomerService(FreelancerContext ctx)
        {
            context = ctx;
        }

        public async Task<bool> InsertOrder(string name, string description)
        {
            int customerId = 1;
            Order order = new Order();
            order.Name = name;
            order.Desc = description;
            order.CustomerId = customerId;
            order.OrderDate = DateTime.UtcNow;
            var res = await context.Orders.AddAsync(order);
            context.SaveChanges();
            return res != null;
        }

        public async Task<List<Order>> GetOrders()
        {
            int id = 1; //TODO GET ID BY AUTHENTICATION
            var customerOrders = await context.Orders.AsAsyncEnumerable().Where(o => o.CustomerId == id).ToListAsync();
            return customerOrders;
        }

        public async Task<bool> AlterOrderStatus(int id, OrderStatus status)
        {
            /*var res = await context.Applications.FirstOrDefaultAsync(a => a.Id == id);
            if (res == null)
                return false;
            res.Status = ApplcationStatus.ACCEPTED;
            context.Applications.Update(res);
            context.SaveChanges();
            context.SaveChanges();*/
            return true;
        }

        public async Task<bool> AcceptApplyFromFreelancer(int application_id)
        {
            /*var res = await context.Applications.FirstOrDefaultAsync(a => a.Id == application_id);
            if (res == null)
                return false;
            res.Status = ApplcationStatus.ACCEPTED;
            context.Applications.Update(res);
            context.SaveChanges();*/
            return true;
        }

        public async Task<bool> AcceptCompletedWork()
        {
            var res = true; //TODO ALTER
            context.SaveChanges();
            return res;
        }
    }
}
