using System;
using System.Collections.Generic;
using System.Net.Http.Json;
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
        
        [HttpGet("~/GetProductsByHeadingOne")]
        public async Task<IEnumerable<Product>> GetProductsByHeadingOne(HeadingOne headingOne)
        {
            return await _service.GetByHeadingOne(headingOne);
        }
        
        [HttpGet("~/GetProductsByHeadingTwo")]
        public async Task<IEnumerable<Product>> GetProductsByHeadingTwo(HeadingTwo headingTwo)
        {
            return await _service.GetByHeadingTwo(headingTwo);
        }
        
        [HttpGet("~/GetProductsByHeadingThree")]
        public async Task<IEnumerable<Product>> GetProductsByHeadingThree(HeadingThree headingThree)
        {
            return await _service.GetByHeadingThree(headingThree);
        }

        [HttpPost("~/AddProduct")]
        public async Task<StatusCodeResult> AddProduct(Product product)
        {
            var success = await _service.Add(product);
            if (success)
                return Ok();
            
            return BadRequest();
        }

        [HttpDelete("~/DeleteProduct")]
        public async Task<StatusCodeResult> DeleteProduct(string productTitle)
        {
            var success = await _service.Delete(productTitle);
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
    }
}