using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesDashBoardApplication.Models.DTO.InventoryDto;
using SalesDashBoardApplication.Services.Contracts;

namespace SalesDashBoardApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoriesController> _logger;

        public InventoriesController(IInventoryService inventoryService, ILogger<InventoriesController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }


        //[HttpGet]
        //public async Task<IActionResult> AllInventoryData()
        //{
        //    var inventories = await _inventoryService.GetAllInventoryItems();
        //    return Ok(inventories);
        //}


        //[HttpGet("low-stock")]
        //public async Task<IActionResult> AllLowStockInventoryData()
        //{
        //    var inventories = await _inventoryService.GetAllLowStockItems();
        //    return Ok(inventories);
        //}


        //[HttpPost("reorder/{productId}")]
        //public async Task<IActionResult> ReorderStock(int productId)
        //{
        //    await _inventoryService.ReorderInventory(productId);
        //    return Ok();
        //}




        [HttpGet]
        public async Task<IEnumerable<InventoryGetDto>> AllInventoryData()
        {
            _logger.LogInformation("Getting all Inventory Details");
            return await _inventoryService.GetAllInventoryItems();
        }



        [HttpGet("low-stock")]
        public async Task<IEnumerable<InventoryGetDto>> LowStockInventoryData()
        {
            _logger.LogInformation("Getting all low stock inventory data");
            return await _inventoryService.GetAllLowStockItems();
        }



        [HttpPost("reorder/{productId}")]
        public async Task ReorderStock(int productId)
        {
            _logger.LogInformation("Re-ordering stock value");
            await _inventoryService.ReorderInventory(productId);
        }
    }
}
