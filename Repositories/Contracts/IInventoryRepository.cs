using SalesDashBoardApplication.Models;

namespace SalesDashBoardApplication.Repositories.Contracts
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllInventoryData();
        Task<IEnumerable<Inventory>> GetAllInventoryOfLowStock();
        Task ReorderInventory(int productId);
    }
}
