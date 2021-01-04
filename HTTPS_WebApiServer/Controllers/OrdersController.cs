using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserMiddleware.Interfaces;
using DataLayer.Models.Database;
using DataLayer.Models.Server;
using DataLayer.Models.DatabaseModels;

namespace HTTPS_WebApiServer.Controllers
{
    [Route("/orders")]
    public class OrdersController : Controller
    {
        private readonly IUserDataService   orders_service;
        private readonly IUserAccessService user_access_service;

        public OrdersController(IUserDataService serv, IUserAccessService uas)
        {
            orders_service      = serv;
            user_access_service = uas;
        }

        /*
         * glitchyhydra.ddns.net/orders/<order_id?>
         * return result of inserting an application
         */
        [HttpPost, Route("/{order_id?}")]
        public async Task<IActionResult> InsertApplication(int? order_id)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            if (order_id is null) return BadRequest("Require order_id for applying");
            else if (!find_token_res) return Unauthorized("Lack of token");
            else
            {
                var freelancer_id = await user_access_service.Authenticate(token, UserActions.InsertApplication);
                if (freelancer_id > -1)
                {
                    var res = await orders_service.InsertApplication(freelancer_id, (int)order_id);
                    return res ? Ok("You succesfully applied to order") : Ok("Error in application inserting");
                }
                else return Unauthorized("Invalid token");
            }
        }

        /*
         * glitchyhydra.ddns.net/orders
         * {name: str, desc: str?}
         * return result of inserting an order
         */
        [HttpPost]
        public async Task<IActionResult> InsertOrder([FromBody] OrderModel model)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            //check if all data present
            if (model is null) return BadRequest("Lack of order name");
            else if (!find_token_res) return Unauthorized("Lack of token");
            //get id and check user data
            var authResult = await user_access_service.Authenticate(token, UserActions.InsertOrder);
            if (authResult > -1)
            {
                var res = await orders_service.InsertOrder(authResult, model);
                return res ? Ok("Order inserted") : Ok("Error in order inserting");
            }
            else return Unauthorized("Invalid token");
        }

        /*
         * glitchyhydra.ddns.net/orders
         * return opened orders if status was not mentioned in body
         * or all orders owned by specific customer 
         */
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromBody] OrderStatus? order_status)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            if (!find_token_res) return Unauthorized("Lack of token");
            //if model not present it is a freelancer request
            //other way it is customer
            OrderStatus status;
            UserActions user_action;
            if (order_status is null)
            {
                status       = OrderStatus.Open;
                user_action  = UserActions.GetOpenedOrders;
            }
            else
            {
                status       = (OrderStatus)order_status;
                user_action  = UserActions.GetOrders;
            }
            //user verification
            var authResult = await user_access_service.Authenticate(token, user_action);
            if (authResult > -1)
            {
                var res = await orders_service.GetOrders(authResult, status);
                return Ok(res);
            }
            else return Unauthorized("Invalid token");
        }

        /*
         * glitchyhydra.ddns.net/orders/&id=number
         * return result of updating order status
         */
        [HttpPut, Route("/{id?}")]
        public async Task<IActionResult> UpdateOrderStatus(int? id, [FromBody] OrderStatus status)
        {
            Microsoft.Extensions.Primitives.StringValues token;
            var find_token_res = Request.Headers.TryGetValue("Authorization", out token);
            if (id is null) return BadRequest("Require order_id for status updating");
            else if (!find_token_res) return Unauthorized("Lack of token");
            else
            {
                var authResult = await user_access_service.Authenticate(token, UserActions.UpdateOrderStatus);
                if (authResult > -1)
                {
                    var res = await orders_service.UpdateOrderStatus((int)id, status);
                    return res? Ok("Updated Sucessfully") : Ok("Error in order status updating");
                }
                else return Unauthorized("Invalid token");
            }
        }
    }
}
