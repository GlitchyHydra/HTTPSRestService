using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using DataLayer.Models.Database;
using DataLayer.Models.Server;
using DataLayer.Models.DatabaseModels;
using System;

namespace HTTPS_WebApiServer.Controllers
{
    /// <summary>
    /// For orders managing
    /// </summary>
    [Controller, Route("/orders")]
    public class OrdersController : Controller
    {
        private readonly IUserDataService orders_service;
        private readonly IUserAccessService user_access_service;

        public OrdersController(IUserDataService serv, IUserAccessService uas)
        {
            orders_service = serv;
            user_access_service = uas;
        }

        /// <summary>
        /// Insert application to an order
        /// </summary>
        /// <param name="order_id">Id of order for applying</param>
        /// <returns></returns>
        /// <response code="200">string answer of inserting result</response>
        /// <response code="401">lack of token in header</response>
        [HttpPost("{order_id:int}")]
        public async Task<IActionResult> InsertApplication(int order_id)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            if (!find_token_res) return Unauthorized(new { erorrText = "Lack of token" });
            else
            {
                var freelancer_id = await user_access_service.Authenticate(token, UserActions.InsertApplication);
                if (freelancer_id > -1)
                {
                    var res = await orders_service.InsertApplication(freelancer_id, order_id);
                    return res ? Ok(new { Answer = "You succesfully applied to order" }) : Ok(new { Answer = "Error in application inserting" });
                }
                else return Unauthorized(new { errorText = "Invalid token" });
            }
        }

        /// <summary>
        /// Add an order
        /// </summary>
        /// <param name="model">Order title and description</param>
        /// <returns></returns>
        /// <response code="200">string answer of inserting result</response>
        /// <response code="401">lack of token in header</response>
        [HttpPost]
        public async Task<IActionResult> InsertOrder([FromBody] OrderModel model)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            //check if all data present
            if (model is null) return BadRequest("Lack of order name");
            else if (!find_token_res) return Unauthorized(new { erorrText = "Lack of token" });
            //get id and check user data
            var authResult = await user_access_service.Authenticate(token, UserActions.InsertOrder);
            if (authResult > -1)
            {
                var res = await orders_service.InsertOrder(authResult, model);
                return res ? Ok(new { Answer = "Order inserted" }) : Ok(new { Answer = "Error in order inserting" });
            }
            else return Unauthorized(new { errorText = "Invalid token" });
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
        public async Task<IActionResult> GetOrders(string status)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            if (!find_token_res) return Unauthorized(new { erorrText = "Lack of token" });
            //if model not present it is a freelancer request
            //other way it is customer

            OrderStatus order_status;
            UserActions user_action;
            if (status == "open")
            {
                order_status = OrderStatus.Open;
                user_action = UserActions.GetOpenedOrders;
            }
            else
            {
                if (status == "processing")
                {
                    order_status = OrderStatus.Processing;
                }
                else
                {
                    order_status = OrderStatus.Close;
                }

                user_action = UserActions.GetOrders;
            }
            //user verification
            var authResult = await user_access_service.Authenticate(token, user_action);
            if (authResult > -1)
            {
                var res = await orders_service.GetOrders(authResult, order_status);
                return Ok(res);
            }
            else return Unauthorized(new { errorText = "Invalid token" });
        }

        /// <summary>
        /// Order status:
        /// "open", "processing", "close"
        /// </summary>
        /// <example>open</example>
        [Serializable]
        public class UpOrdStat 
        {
            public string status { get; set; }
        };

        /// <summary>
        /// update the status of the order
        /// </summary>
        /// <param name="id">id of the order for updating</param>
        /// <param name="up_status">new status</param>
        /// <returns></returns>
        /// <response code="200">string answer of updating result</response>
        /// <response code="401">lack of token in header</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpOrdStat up_status)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);  
            if (!find_token_res) return Unauthorized(new { erorrText = "Lack of token" });
            if (up_status is null) return BadRequest(new { errorText = "Order status required" });
            else
            {
                var authResult = await user_access_service.Authenticate(token, UserActions.UpdateOrderStatus);
                if (authResult > -1)
                {
                    OrderStatus order_status = (OrderStatus)System.Enum.Parse(typeof(OrderStatus), up_status.status, true);
                    var res = await orders_service.UpdateOrderStatus((int)id, order_status);
                    return res ? Ok(new { Answer = "Updated Sucessfully" }) : Ok(new { Answer = "Error in order status updating" });
                }
                else return Unauthorized(new { errorText = "Invalid token" });
            }
        }
    }
}
