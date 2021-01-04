using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models.Database;
using DataLayer.Models.Server;
using DataLayer;
using UserMiddleware.Interfaces;

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
                Name = order_model.name,
                Desc = order_model.desc,
                CustomerId = customer_id,
                OrderDate = DateTime.UtcNow
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

        /*----------------------------------Select--------------------------------------*/
        public async Task<List<Order>> GetOrders(int customer_id, OrderStatus status)
        {
            return await context.Orders
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
            var res = await context.Orders.FirstOrDefaultAsync(o => o.Id == order_id);
            if (res == null) return false;
            else
            {
                res.Status = status;
                context.Orders.Update(res);
                context.SaveChanges();
                return true;
            }
        }

        public async Task<bool> UpdateApplicationStatus(int application_id, ApplcationStatus status)
        {
            var res = await context.Applications.FirstOrDefaultAsync(a => a.Id == application_id);
            if (res == null) return false;
            else
            {
                res.Status = status;
                context.Applications.Update(res);
                context.SaveChanges();
                return true;
            }
        }

        public async Task<bool> UpdateWorkStatus(int work_id, WorkStatus status)
        {
            var res = await context.Works.FirstOrDefaultAsync(w => w.Id == work_id);
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
