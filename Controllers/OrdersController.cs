using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesDashBoardApplication.Models;
using SalesDashBoardApplication.Models.DTO.OrderDto;
using SalesDashBoardApplication.Services.Contracts;

namespace SalesDashBoardApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }


        //[HttpGet]
        //public async Task<IActionResult> GetAllOrders()
        //{
        //    var orders = await _orderService.GetAllOrders();
        //    return Ok(orders);
        //}


        //[HttpGet("user/{userId}")]
        //public async Task<IActionResult> OrdersOfUser(int userId)
        //{
        //    var orders = await _orderService.GetOrdersOfUser(userId);
        //    return Ok(orders);
        //}


        //[HttpPost]
        //public async Task<IActionResult> CreateOrder(OrderAddDto orderAddDto)
        //{
        //    var order = new Order
        //    {
        //        UserId = orderAddDto.UserId,
        //        OrderItems = new List<OrderItem>()
        //    };

        //    double totalAmount = 0;

        //    foreach (var item in orderAddDto.OrderItems)
        //    {
        //        var orderItem = new OrderItem
        //        {
        //            ProductId = item.ProductId,
        //            Quantity = item.Quantity,
        //            UnitPrice = item.UnitPrice
        //        };

        //        order.OrderItems.Add(orderItem);
        //        totalAmount += (orderItem.Quantity * orderItem.UnitPrice);
        //    }

        //    order.OrderAmount = totalAmount;

        //    await _orderService.CreateOrder(order);
        //    return Created();
        //}




        [HttpGet]
        public async Task<IEnumerable<OrderGetDto>> GetAllOrders()
        {
            _logger.LogInformation("Getting all orders data");
            return await _orderService.GetAllOrders();
        }



        [HttpGet("user/{userId}")]
        public async Task<IEnumerable<OrderGetDto>> GetOrdersOfUsers(int userId)
        {
            _logger.LogInformation("Getting orders of a user");
            return await _orderService.GetOrdersOfUser(userId);
        }



        [HttpPost]
        public async Task CreateOrder(OrderAddDto orderAddDto)
        {
            var order = new Order
            {
                UserId = orderAddDto.UserId,
                OrderItems = new List<OrderItem>()
            };

            double totalAmount = 0;

            foreach (var item in orderAddDto.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };

                order.OrderItems.Add(orderItem);
                totalAmount += (orderItem.Quantity * orderItem.UnitPrice);
            }

            order.OrderAmount = totalAmount;

            _logger.LogInformation("Creating a new Order");
            await _orderService.CreateOrder(order);
        }
    }
}
