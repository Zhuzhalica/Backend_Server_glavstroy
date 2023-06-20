using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackServer.Services.Interfaces;
using Entity;
using Microsoft.Extensions.Logging;

namespace BackServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsHandlersController : ControllerBase
    {
        private readonly ILogger<ProductsHandlersController> _logger;
        private readonly IProductService _service;

        public ProductsHandlersController(ILogger<ProductsHandlersController> logger, IProductService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("~/GetAllProducts")]
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _service.GetAll();
        }
        
        
        [HttpGet("~/GetAvailableProducts")]
        public async Task<IEnumerable<Product>> GetAvailableProducts()
        {
            return await _service.GetAvailable();
        }
        
        [HttpGet("~/GetProductsByHeadingOne")]
        public async Task<IEnumerable<Product>> GetProductsByHeadingOne(string headingOneTitle, [FromQuery]HashSet<Property> requiredProperties, int pageNumber, int countElements)
        {
            return await _service.GetByHeadingOne(headingOneTitle, requiredProperties.ToHashSet(), pageNumber, countElements);
        }
        
        [HttpGet("~/GetProductsByHeadingTwo")]
        public async Task<IEnumerable<Product>> GetProductsByHeadingTwo(string headingTwoTitle, int pageNumber, int countElements)
        {
            return await _service.GetByHeadingTwo(headingTwoTitle, pageNumber, countElements);
        }
        
        [HttpGet("~/GetProductsByHeadingThree")]
        public async Task<IEnumerable<Product>> GetProductsByHeadingThree(string headingThreeTitle, int pageNumber, int countElements)
        {
            return await _service.GetByHeadingThree(headingThreeTitle, pageNumber, countElements);
        }

        [HttpPost("~/AddProduct")]
        public async Task<StatusCodeResult> AddProduct(Product product)
        {
            var success = await _service.Add(product);
            if (success)
                return Ok();
            
            return BadRequest();
        }

        [HttpDelete("~/DeleteProducts")]
        public async Task<StatusCodeResult> DeleteProduct(HashSet<string> productTitles)
        {
            var success = await _service.Delete(productTitles);
            if (success)
                return Ok();
            
            return BadRequest();
        }
        
        [HttpPost("~/UpdateProduct")]
        public async Task<StatusCodeResult> UpdateProduct(string oldProductTitle, Product product)
        {
            var success = await _service.Update(oldProductTitle, product);
            if (success)
                return Ok();
            
            return BadRequest();
        }
        
        [HttpDelete("~/DeleteHeadingOneProducts")]
        public async Task<StatusCodeResult> DeleteHeadingOneProducts(string headingOneTitle)
        {
            var success = await _service.DeleteHeadingOneProducts(headingOneTitle);
            if (success)
                return Ok();
            
            return BadRequest();
        }
        
        [HttpDelete("~/DeleteHeadingTwoProducts")]
        public async Task<StatusCodeResult> DeleteHeadingTwoProducts(string headingTwoTitle)
        {
            var success = await _service.DeleteHeadingTwoProducts(headingTwoTitle);
            if (success)
                return Ok();
            
            return BadRequest();
        }
        
        [HttpDelete("~/DeleteHeadingThreeProducts")]
        public async Task<StatusCodeResult> DeleteHeadingThreeProducts(string headingThreeTitle)
        {
            var success = await _service.DeleteHeadingThreeProducts(headingThreeTitle);
            if (success)
                return Ok();
            
            return BadRequest();
        }
    }
}