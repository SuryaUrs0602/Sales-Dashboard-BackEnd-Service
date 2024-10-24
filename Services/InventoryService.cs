﻿using Microsoft.EntityFrameworkCore;
using SalesDashBoardApplication.Models;
using SalesDashBoardApplication.Models.DTO.InventoryDto;
using SalesDashBoardApplication.Repositories.Contracts;
using SalesDashBoardApplication.Services.Contracts;
using System.Text.RegularExpressions;

namespace SalesDashBoardApplication.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILogger<InventoryService> _logger;
        private readonly MqttService _mqttService;

        public InventoryService(IInventoryRepository inventoryRepository, ILogger<InventoryService> logger, MqttService mqttService)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
            _mqttService = mqttService;
        }

        public async Task<IEnumerable<InventoryGetDto>> GetAllInventoryItems()
        {
            try
            {
                _logger.LogInformation("Attempting to fetch all inventory items");
                var inventories = await _inventoryRepository.GetAllInventoryData();
                return inventories.Select(x => new InventoryGetDto
                {
                    InventoryId = x.InventoryId,
                    StockLevel = x.StockLevel,
                    ProductId = x.Product.ProductId,
                    ProductName = x.Product.ProductName,
                    ProductCategory = x.Product.ProductCategory,
                    ProductPrice = x.Product.ProductPrice,
                });
            }
            
            catch (ArgumentNullException argEx)
            {
                _logger.LogError(argEx, "An argument null exception occurred while fetching inventory items.");
                throw new ArgumentNullException("An error occurred: a required argument was missing.");
            }

            catch (OperationCanceledException opEx)
            {
                _logger.LogError(opEx, "An operation canceled exception occurred while fetching inventory items.");
                throw new OperationCanceledException("An error occurred: a required argument was missing.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching inventory items.");
                throw new Exception("An error occurred while retrieving inventory items. Please try again later.");
            }
        }

        public async Task<IEnumerable<InventoryGetDto>> GetAllLowStockItems()
        {
            try
            {
                _logger.LogInformation("Attempting to fetch all inventory items");
                var inventories = await _inventoryRepository.GetAllInventoryOfLowStock();
                return inventories.Select(x => new InventoryGetDto
                {
                    InventoryId = x.InventoryId,
                    StockLevel = x.StockLevel,
                    ProductId = x.Product.ProductId,
                    ProductName = x.Product.ProductName,
                    ProductCategory = x.Product.ProductCategory,
                    ProductPrice = x.Product.ProductPrice,
                });
            }

            catch (ArgumentNullException argEx)
            {
                _logger.LogError(argEx, "An argument null exception occurred while fetching low-stock items.");
                throw new ArgumentNullException("An error occurred: a required argument was missing.");
            }

            catch (OperationCanceledException opEx)
            {
                _logger.LogError(opEx, "An operation canceled exception occurred while fetching low-stock items.");
                throw new OperationCanceledException("An error occurred: a required argument was missing.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching low-stock items.");
                throw new Exception("An error occurred while retrieving inventory items. Please try again later.");
            }

        }

        public async Task ReorderInventory(int productId)
        {
            try
            {
                _logger.LogInformation("Attempting to Update the Stock level");
                await _inventoryRepository.ReorderInventory(productId);

                // publishing message to mqtt topic
                await _mqttService.PublishUpdateAsync("ecommerce/stock-update", productId);
            }

            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "A database update error occurred while re-ordering item.");
                throw new DbUpdateException("An error occurred while accessing the database. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while re-ordering item.");
                throw new Exception("An error occurred while retrieving inventory items. Please try again later.");
            }
        }
    }
}
