using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesDashBoardApplication.Models;
using SalesDashBoardApplication.Models.DTO.ProductDto;
using SalesDashBoardApplication.Services.Contracts;

namespace SalesDashBoardApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }


        //[HttpPost]
        //public async Task<IActionResult> AddProduct(ProductDto productDto)
        //{
        //    var product = new Product
        //    {
        //        ProductName = productDto.ProductName,
        //        ProductImageUrl = productDto.ProductImageUrl,
        //        ProductDescription = productDto.ProductDescription,
        //        ProductCategory = productDto.ProductCategory,
        //        ProductPrice = productDto.ProductPrice
        //    };
        //    await _productService.AddProduct(product);
        //    return Created();
        //}


        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    await _productService.DeleteProduct(id);
        //    return NoContent();
        //}


        //[HttpGet]
        //public async Task<IActionResult> GetAllProducts()
        //{
        //    var products = await _productService.GetAllProducts();
        //    return Ok(products);
        //}


        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetProductById(int id)
        //{
        //    var product = await _productService.GetProductById(id);
        //    if (product == null) 
        //        return NotFound();
        //    return Ok(product);
        //}


        //[HttpGet("category/{category}")]
        //public async Task<IActionResult> GetProductsByCategory(string category)
        //{
        //    var products = await _productService.GetProductsByCategory(category);
        //    return Ok(products);
        //}


        //[HttpPatch("{id}")]
        //public async Task<IActionResult> UpdateProduct(int id, JsonPatchDocument<ProductDto> patchDocument)
        //{
        //    if (patchDocument == null)
        //        return BadRequest();

        //    var existingProduct = await _productService.FindProduct(id);

        //    if (existingProduct == null)
        //        return NotFound();
       
        //    var productDto = new ProductDto
        //    {
        //        ProductName = existingProduct.ProductName,
        //        ProductImageUrl = existingProduct.ProductImageUrl,
        //        ProductDescription = existingProduct.ProductDescription,
        //        ProductCategory = existingProduct.ProductCategory,
        //        ProductPrice = existingProduct.ProductPrice
        //    };

        //    patchDocument.ApplyTo(productDto, ModelState);

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    existingProduct.ProductName = productDto.ProductName;
        //    existingProduct.ProductImageUrl = productDto.ProductImageUrl;
        //    existingProduct.ProductDescription = productDto.ProductDescription;
        //    existingProduct.ProductCategory = productDto.ProductCategory;
        //    existingProduct.ProductPrice = productDto.ProductPrice;

        //    await _productService.UpdateProduct(existingProduct);
        //    return NoContent();
        //}


        [HttpPost]
        public async Task AddProduct(ProductDto productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                ProductImageUrl = productDto.ProductImageUrl,
                ProductDescription = productDto.ProductDescription,
                ProductCategory = productDto.ProductCategory,
                ProductPrice = productDto.ProductPrice
            };

            _logger.LogInformation("Creating a new Product");
            await _productService.AddProduct(product);
        }



        [HttpDelete("{id}")]
        public async Task DeleteProduct(int id)
        {
            _logger.LogInformation("Deleting the product");
            await _productService.DeleteProduct(id);
        }



        [HttpGet]
        public async Task<IEnumerable<ProductGetDto>> GetAllProducts()
        {
            _logger.LogInformation("Getting all Products data");
            return await _productService.GetAllProducts();
        }



        [HttpGet("{id}")]
        public async Task<ProductGetDto> GetProductById(int id)
        {
            _logger.LogInformation("Getting product by ID");
            return await _productService.GetProductById(id);
        }


                    
        [HttpGet("category/{category}")]
        public async Task<IEnumerable<ProductGetDto>> GetProductsByCategory(string category)
        {
            _logger.LogInformation("Getting all products of a particular category");
            return await _productService.GetProductsByCategory(category);
        }



        [HttpPatch("{id}")]
        public async Task UpdateProduct(int id, JsonPatchDocument<ProductDto> patchDocument)
        {
            if (patchDocument == null)
                throw new ApplicationException("Cannot Update Product Try after some time");

            var existingProduct = await _productService.FindProduct(id);

            if (existingProduct == null)
                throw new ApplicationException("Cannot Find Product Try after some time");

            var productDto = new ProductDto
            {
                ProductName = existingProduct.ProductName,
                ProductImageUrl = existingProduct.ProductImageUrl,
                ProductDescription = existingProduct.ProductDescription,
                ProductCategory = existingProduct.ProductCategory,
                ProductPrice = existingProduct.ProductPrice
            };

            patchDocument.ApplyTo(productDto, ModelState);

            if (!ModelState.IsValid)
                throw new ApplicationException("Cannot Update Product due some issue");

            existingProduct.ProductName = productDto.ProductName;
            existingProduct.ProductImageUrl = productDto.ProductImageUrl;
            existingProduct.ProductDescription = productDto.ProductDescription;
            existingProduct.ProductCategory = productDto.ProductCategory;
            existingProduct.ProductPrice = productDto.ProductPrice;

            _logger.LogInformation("Updating the Product");
            await _productService.UpdateProduct(existingProduct);
        }
    }
}
