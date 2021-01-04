using System.Collections.Generic;
using System.Threading.Tasks;
using DataLayer.Models.Database;
using DataLayer.Models.Server;

namespace UserMiddleware.Interfaces
{
    public interface IUserDataService
    {
        /*----------------------------------Insert--------------------------------------*/
        public Task<bool> InsertOrder(int customer_id, OrderModel order_model);
        public Task<bool> InsertApplication(int freelancer_id, int order_id);

        /*----------------------------------Select--------------------------------------*/
        public Task<List<Order>> GetOrders(int customer_id, OrderStatus status);
        public Task<List<Application>> GetApplications(int freelancer_id);
        public Task<List<Work>> GetWorks(int freelancer_id);

        /*----------------------------------Update--------------------------------------*/
        public Task<bool> UpdateOrderStatus(int order_id, OrderStatus status);
        public Task<bool> UpdateApplicationStatus(int application_id, ApplcationStatus status);
        public Task<bool> UpdateWorkStatus(int work_id, WorkStatus status);
    }
}
