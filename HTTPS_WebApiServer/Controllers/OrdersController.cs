using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using FreelancerWeb.DataLayer.Models.Database;
using FreelancerWeb.DataLayer.Models.Server;
using System;
using Microsoft.AspNetCore.Authorization;
using FreelancerWeb.Authorization;
using System.Linq;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

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
        /// <response code="200">Application to order was inserted</response>
        /// <response code="500">Error in application inserting</response>
        [HttpPost("{order_id:int}")]
        [Authorize(Roles = WebRoles.Freelancer)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(FreelancerResponse), Description = "Application to order was inserted")]
        public async Task<IActionResult> InsertApplication(int order_id)
        {
            var freelancerId = Id;
            if (freelancerId == NotAuthroized) return Unauthorized(new FreelancerResponse { Message = TokenInvalidAnswer });
            var res = await orders_service.InsertApplication(freelancerId, order_id);
            if (res) return Ok(new FreelancerResponse { Message = "You successfully applied to order" });
            else     return StatusCode(StatusCodes.Status500InternalServerError,
                new FreelancerResponse() { Message = "Error in application inserting" });
        }

        /// <summary>
        /// Add an order
        /// </summary>
        /// <remarks>
        /// POST /orders
        /// {
        ///     "Name": "Order1",
        ///     "Desc": "Empty Description"
        /// }
        /// </remarks>
        /// <param name="model">Order title and description</param>
        /// <returns></returns>
        /// <response code="200">Order was inserted</response>
        /// <response code="400">Need to specify an order name in model</response>
        /// <response code="500">Error in order inserting</response>
        [HttpPost]
        [Authorize(Roles = WebRoles.Customer)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(FreelancerResponse), Description = "Order was insterted")]
        public async Task<IActionResult> InsertOrder([FromBody] OrderModel model)
        {
            if (model is null) return BadRequest(new FreelancerResponse { Message = "Lack of order name" });
            var customerId = Id;
            if (customerId == NotAuthroized) return Unauthorized(new FreelancerResponse{ Message = TokenInvalidAnswer });
            var res = await orders_service.InsertOrder(customerId, model);
            if (res) return Ok(new FreelancerResponse { Message = "Order was inserted" });
            else return StatusCode(StatusCodes.Status500InternalServerError, 
                new FreelancerResponse() { Message = "Error in order inserting" });
        }

        /// <summary>
        /// Get all orders owned by a customer id and filtered by the status. 
        /// When freelancer is authorized, return only opened orders. 
        /// When customer is authorized, return owned orders by status filter.
        /// </summary>
        /// <param name="status">Status filter("open", "processing", "close")</param>
        /// <returns></returns>
        /// <response code="200">Orders owned by a customer
        /// or opened orders if freelancer is logged</response>
        [HttpGet("{status?}")]
        [Authorize()]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IList<Order>), Description = "Orders by status")]
        public async Task<IActionResult> GetOrders(string status)
        {
            OrderStatus order_status = Enum.GetNames(typeof(OrderStatus)).Contains(status) 
                ? (OrderStatus)Enum.Parse(typeof(OrderStatus), status, true)
                : OrderStatus.Open;
            UserActions user_action; 
            if (order_status == OrderStatus.Open) user_action = UserActions.GetOpenedOrders;
            else user_action = UserActions.GetOrders;
            UserRole role = GetRole();
            var user_id = Id;
            if (!UserActionManager.IsAuthorized(role, user_action) || Id == NotAuthroized)
                return Unauthorized(new FreelancerResponse{ Message = TokenInvalidAnswer });
            var res = role == UserRole.Customer 
                ? await orders_service.GetOrdersByCustomerId(user_id, order_status)
                : await orders_service.GetOrders(order_status);
            return Ok(res);
        }

        /// <summary>
        /// update the status of the order
        /// </summary>
        /// <param name="id">id of the order for updating</param>
        /// <param name="up_status">new status</param>
        /// <returns></returns>
        /// <response code="200">Order status was updated</response>
        /// <response code="400">Need to specify an order status</response>
        /// <response code="500">Error in order status updating</response>
        [HttpPut("{id:int}")]
        [Authorize(Roles = WebRoles.Customer)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(FreelancerResponse), Description = "Order status was updated")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] StatusUpdateModel up_status)
        {
            if (up_status is null) return BadRequest(new FreelancerResponse{ Message = "Order status required" });
            var customerId = Id;
            if (customerId == NotAuthroized) return Unauthorized(new FreelancerResponse{ Message = TokenInvalidAnswer });
            OrderStatus order_status = (OrderStatus)System.Enum.Parse(typeof(OrderStatus), up_status.status, true);
            var res = await orders_service.UpdateOrderStatus((int)id, order_status);
            if (res) return Ok(new FreelancerResponse { Message = "Order status was updated" });
            else return StatusCode(StatusCodes.Status500InternalServerError,
                new FreelancerResponse() { Message = "Error in order status updating" });
        }
    }
}
