using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using FreelancerWeb.DataLayer.Models.Database;
using FreelancerWeb.DataLayer.Models.Server;
using System;
using Microsoft.AspNetCore.Authorization;
using FreelancerWeb.Authorization;
using System.Linq;

namespace FreelancerWeb.Controllers
{
    /// <summary>
    /// For orders managing
    /// </summary>
    [Controller, Route("/orders")]
    public class OrdersController : BaseController
    {
        private readonly IUserDataService orders_service;

        public OrdersController(IUserDataService serv)
        {
            orders_service = serv;
        }

        /// <summary>
        /// Insert application to an order
        /// </summary>
        /// <param name="order_id">Id of order for applying</param>
        /// <returns></returns>
        /// <response code="200">string answer of inserting result</response>
        /// <response code="401">lack of token in header</response>
        [HttpPost("{order_id:int}")]
        [Authorize(Roles = WebRoles.Freelancer)]
        public async Task<IActionResult> InsertApplication(int order_id)
        {
            var freelancerId = Id;
            if (freelancerId == NotAuthroized)
                return Unauthorized(new { errorText = TokenInvalidAnswer });
            var res = await orders_service.InsertApplication(freelancerId, order_id);
            return res ? Ok(new { Answer = "You succesfully applied to order" })
                : Ok(new { Answer = "Error in application inserting" });
        }

        /// <summary>
        /// Add an order
        /// </summary>
        /// <param name="model">Order title and description</param>
        /// <returns></returns>
        /// <response code="200">string answer of inserting result</response>
        /// <response code="401">lack of token in header</response>
        [HttpPost]
        [Authorize(Roles = WebRoles.Customer)]
        public async Task<IActionResult> InsertOrder([FromBody] OrderModel model)
        {
            if (model is null) return BadRequest("Lack of order name");
            var customerId = Id;
            if (customerId == NotAuthroized)
                return Unauthorized(new { errorText = "Invalid token" });
            var res = await orders_service.InsertOrder(customerId, model);
            return res ? Ok(new { Answer = "Order inserted" }) : Ok(new { Answer = "Error in order inserting" });
        }

        /// <summary>
        /// Get all orders owned by a customer id and filtered by the status. 
        /// When freelancer is authorized, return only opened orders. 
        /// When customer is authorized, return owned orders by status filter.
        /// </summary>
        /// <param name="status">Status filter("open", "processing", "close")</param>
        /// <returns></returns>
        /// <response code="200">orders</response>
        /// <response code="401">lack of token in header</response>
        [HttpGet("{status?}")]
        [Authorize()]
        public async Task<IActionResult> GetOrders(string status)
        {
            OrderStatus order_status = Enum.GetNames(typeof(OrderStatus)).Contains(status) 
                ? (OrderStatus)Enum.Parse(typeof(OrderStatus), status, true)
                : OrderStatus.Open;
            UserActions user_action; 
            if (order_status == OrderStatus.Open) user_action = UserActions.GetOpenedOrders;
            else user_action = UserActions.GetOrders;
            UserRole role = GetRole();
            if (!UserActionManager.IsAuthorized(role, user_action))
                return Unauthorized(new { errorText = TokenInvalidAnswer });
            var user_id = Id;
            if (Id == NotAuthroized)
                return Unauthorized(new { errorText = TokenInvalidAnswer });
            var res = await orders_service.GetOrders(user_id, order_status);
            return Ok(res);
        }

        /// <summary>
        /// update the status of the order
        /// </summary>
        /// <param name="id">id of the order for updating</param>
        /// <param name="up_status">new status</param>
        /// <returns></returns>
        /// <response code="200">string answer of updating result</response>
        /// <response code="401">lack of token in header</response>
        [HttpPut("{id:int}")]
        [Authorize(Roles = WebRoles.Customer)]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] StatusUpdateModel up_status)
        {
            if (up_status is null) return BadRequest(new { errorText = "Order status required" });
            var customerId = Id;
            if (customerId == NotAuthroized)
                return Unauthorized(new { errorText = TokenInvalidAnswer });
            OrderStatus order_status = (OrderStatus)System.Enum.Parse(typeof(OrderStatus), up_status.status, true);
            var res = await orders_service.UpdateOrderStatus((int)id, order_status);
            return res 
                ? Ok(new { Answer = "Updated Sucessfully" })
                : Ok(new { Answer = "Error in order status updating" });
        }
    }
}
