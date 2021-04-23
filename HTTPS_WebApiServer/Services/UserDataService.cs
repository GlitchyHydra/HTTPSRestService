using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FreelancerWeb.DataLayer.Models.Database;
using FreelancerWeb.DataLayer.Models.Server;
using DataLayer;
using UserMiddleware.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace UserMiddleware.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly FreelancerContext context;

        public UserDataService(FreelancerContext ctx)
        {
            context = ctx;
        }

        /*----------------------------------Insert--------------------------------------*/
        public async Task<bool> InsertOrder(int customer_id, OrderModel order_model)
        {
            Order order = new Order
            {
                Name = order_model.Name,
                Desc = order_model.Desc,
                CustomerId = customer_id,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Open
            };
            var res = await context.Orders.AddAsync(order);
            context.SaveChanges();
            return res != null;
        }

        public async Task<bool> InsertApplication(int freelancer_id, int order_id)
        {
            Application application = new Application
            {
                OrderId = order_id,
                Status = ApplcationStatus.Open,
                FreelancerId = freelancer_id,
                ApplyDate = DateTime.UtcNow
            };
            var res = await context.Applications.AddAsync(application);
            context.SaveChanges();
            return res != null;
        }

        public async Task<bool> InsertWork(int freelancer_id, int order_id)
        {
            Work work = new Work
            {
                OrderId = order_id,
                Status = WorkStatus.Open,
                FreelancerId = freelancer_id,
                WorkDate = DateTime.UtcNow
            };
            var res = await context.Works.AddAsync(work);
            context.SaveChanges();
            return res != null;
        }

        /*----------------------------------Select--------------------------------------*/
        public async Task<List<Order>> GetOrders(OrderStatus status)
        {
            return await context.Orders.Include("Applications")
                .AsAsyncEnumerable()
                .Where(o => o.Status == status)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByCustomerId(int customer_id, OrderStatus status)
        {
            return await context.Orders.Include("Applications")
                .AsAsyncEnumerable()
                .Where(o => o.CustomerId == customer_id && o.Status == status)
                .ToListAsync();
        }

        public async Task<List<Application>> GetApplications(int freelancer_id)
        {
            return await context.Applications
                .AsAsyncEnumerable()
                .Where(a => a.Id == freelancer_id)
                .ToListAsync();
        }

        public async Task<List<Work>> GetWorks(int freelancer_id)
        {
            return await context.Works
                .AsAsyncEnumerable()
                .Where(w => w.FreelancerId == freelancer_id)
                .ToListAsync();
        }

        /*----------------------------------Update--------------------------------------*/
        public async Task<bool> UpdateOrderStatus(int order_id, OrderStatus status)
        {
            
            var res = await context.Orders.AsAsyncEnumerable().FirstOrDefaultAsync(o => o.Id == order_id);
            if (res == null) return false;
            else
            {
                res.Status = status;
                //context.Entry(res).CurrentValues.SetValues(res);
                context.Update(res);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateApplicationStatus(int application_id, ApplcationStatus status)
        {
            var application = await context.Applications.AsAsyncEnumerable().FirstOrDefaultAsync(a => a.Id == application_id);
            if (application == null) return false;
            else
            {
                application.Status = status;
                context.Applications.Update(application);
                context.SaveChanges();
                if (status == ApplcationStatus.Accepted)
                    await InsertWork(application.FreelancerId, application.OrderId);
                return true;
            }
        }

        public async Task<bool> UpdateWorkStatus(int work_id, WorkStatus status)
        {
            var res = await context.Works.AsAsyncEnumerable().FirstOrDefaultAsync(w => w.Id == work_id);
            if (res == null) return false;
            else
            {
                res.Status = status;
                context.Works.Update(res);
                context.SaveChanges();
                return true;
            }
        }
    }
}
