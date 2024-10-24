using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesDashBoardApplication.Models.DTO.RevenueDto;
using SalesDashBoardApplication.Services.Contracts;

namespace SalesDashBoardApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenuesController : ControllerBase
    {
        private readonly IRevenueService _revenueService;
        private readonly ILogger<RevenuesController> _logger;

        public RevenuesController(IRevenueService revenueService, ILogger<RevenuesController> logger)
        {
            _revenueService = revenueService;
            _logger = logger;
        }



        //[HttpGet("total-revenue/{year}")]
        //public async Task<IActionResult> RevenueOfYear(int year)
        //{
        //    var yearRevenue = await _revenueService.RevenueInYear(year);
        //    return Ok(yearRevenue);
        //}



        //[HttpGet("total-revenue/{startYear}/{endYear}")]
        //public async Task<IActionResult> RevenueInRange(int startYear, int endYear)
        //{
        //    var revenueInRange = await _revenueService.RevenueInRange(startYear, endYear);
        //    return Ok(revenueInRange);
        //}



        //[HttpGet("revenue-per-order/{year}")]
        //public async Task<IActionResult> RevenuePerOrderOfYear(int year)
        //{
        //    var revenuePerOrders = await _revenueService.RevenuePerOrderInYear(year);
        //    return Ok(revenuePerOrders);
        //}



        //[HttpGet("revenue-per-order/{startYear}/{endYear}")]
        //public async Task<IActionResult> RevenuePerOrderInrange(int startYear, int endYear)
        //{
        //    var revenuePerOrders = await _revenueService.RevenuePerOrderInRange(startYear, endYear);
        //    return Ok(revenuePerOrders);
        //}



        //[HttpGet("revenue-growth-rate/{year}")]
        //public async Task<IActionResult> RevenueGrowthRateOfYear(int year)
        //{
        //    var revenueGrowthData = await _revenueService.RevenueGrowthOfYear(year);
        //    return Ok(revenueGrowthData);
        //}



        //[HttpGet("revenue-growth-rate/{startYear}/{endYear}")]
        //public async Task<IActionResult> RevenueGrowthRateInRange(int startYear, int endYear)
        //{
        //    var revenueGrowthData = await _revenueService.RevenueGrowthInRange(startYear, endYear);
        //    return Ok(revenueGrowthData);
        //}



        //[HttpGet("total-cost/{year}")]
        //public async Task<IActionResult> TotalCostInvestedInYear(int year)
        //{
        //    var totalCost = await _revenueService.TotalCostOfYear(year);
        //    return Ok(totalCost);
        //}



        //[HttpGet("total-cost/{startYear}/{endYear}")]
        //public async Task<IActionResult> TotalCostInvestedInRangeOfYear(int startYear, int endYear)
        //{
        //    var totalCost = await _revenueService.TotalCostInRange(startYear, endYear);
        //    return Ok(totalCost);
        //}




        //[HttpGet("cost-per-order/{year}")]
        //public async Task<IActionResult> CostPerOrderInYear(int year)
        //{
        //    var costPerOrder = await _revenueService.CostPerOrderInYear(year);
        //    return Ok(costPerOrder);
        //}



        //[HttpGet("cost-per-order/{startYear}/{endYear}")]
        //public async Task<IActionResult> CostPerOrderInRange(int startYear, int endYear)
        //{
        //    var costPerOrder = await _revenueService.CostPerOrderInRange(startYear, endYear);
        //    return Ok(costPerOrder);
        //}



  






        [HttpGet("total-revenue/{year}")]
        public async Task<IEnumerable<TotalRevenueDto>> RevenueOfYear(int year)
        {
            _logger.LogInformation("Getting revenue data of a year");
            return await _revenueService.RevenueInYear(year);
        }



        [HttpGet("total-revenue/{startYear}/{endYear}")]
        public async Task<IEnumerable<TotalRevenueDto>> RevenueInRange(int startYear, int endYear)
        {
            _logger.LogInformation("Getting revenue data in a range of years");
            return await _revenueService.RevenueInRange(startYear, endYear);            
        }



        [HttpGet("revenue-per-order/{year}")]
        public async Task<IEnumerable<RevenuePerOrderDto>> RevenuePerOrderOfYear(int year)
        {
            _logger.LogInformation("Getting revenue per order data of a year");
            return await _revenueService.RevenuePerOrderInYear(year);
        }



        [HttpGet("revenue-per-order/{startYear}/{endYear}")]
        public async Task<IEnumerable<RevenuePerOrderDto>> RevenuePerOrderInrange(int startYear, int endYear)
        {
            _logger.LogInformation("Getting revenue per order data in arange of years");
            return await _revenueService.RevenuePerOrderInRange(startYear, endYear);
        }



        [HttpGet("revenue-growth-rate/{year}")]
        public async Task<IEnumerable<RevenueGrowthRateDto>> RevenueGrowthRateOfYear(int year)
        {
            _logger.LogInformation("Getting revenue growth data of a year");
            return await _revenueService.RevenueGrowthOfYear(year);
        }



        [HttpGet("revenue-growth-rate/{startYear}/{endYear}")]
        public async Task<IEnumerable<RevenueGrowthRateDto>> RevenueGrowthRateInRange(int startYear, int endYear)
        {
            _logger.LogInformation("Getting revenue growth data in a range of years");
            return await _revenueService.RevenueGrowthInRange(startYear, endYear);
        }



        [HttpGet("total-cost/{year}")]
        public async Task<IEnumerable<TotalCostDto>> TotalCostInvestedInYear(int year)
        {
            _logger.LogInformation("Getting total cost of a year");
            return await _revenueService.TotalCostOfYear(year);
        }



        [HttpGet("total-cost/{startYear}/{endYear}")]
        public async Task<IEnumerable<TotalCostDto>> TotalCostInvestedInRangeOfYear(int startYear, int endYear)
        {
            _logger.LogInformation("Getting total cost in a range of years");
            return await _revenueService.TotalCostInRange(startYear, endYear);
        }




        [HttpGet("cost-per-order/{year}")]
        public async Task<IEnumerable<CostPerOrderDto>> CostPerOrderInYear(int year)
        {
            _logger.LogInformation("Getting the cost per order data of a year");
            return await _revenueService.CostPerOrderInYear(year);      
        }



        [HttpGet("cost-per-order/{startYear}/{endYear}")]
        public async Task<IEnumerable<CostPerOrderDto>> CostPerOrderInRange(int startYear, int endYear)
        {
            _logger.LogInformation("Getting all cost per order data in a range of years");
            return await _revenueService.CostPerOrderInRange(startYear, endYear);
        }
    }
}
