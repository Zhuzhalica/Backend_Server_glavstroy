using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackServer.Repositories;
using BackServer.Services.Interfaces;
using Entity;
using Microsoft.Extensions.Logging;

namespace BackServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesHandlersController : ControllerBase
    {
        private readonly ILogger<SalesHandlersController> _logger;
        private readonly ISaleService _service;

        public SalesHandlersController(ILogger<SalesHandlersController> logger, ISaleService service)
        {
            _logger = logger;
            _service = service;
        }
        
        [HttpGet("~/GetAllSales")]
        public async Task<IEnumerable<Sale>> GetAllSales()
        {
            return await _service.GetAllSales();
        }
        
        [HttpGet("~/GetProductsBySale")]
        public async Task<IEnumerable<Product>> GetProductsBySale(string saleTitle)
        {
            return await _service.GetProductsBySale(saleTitle);
        }
        
        [HttpPost("~/AddSale")]
        public async Task<StatusCodeResult> Add(Sale sale)
        {
            var success = await _service.Add(sale);
            if (success)
                return Ok();
            return BadRequest();
        }
        
        [HttpDelete("~/DeleteSale")]
        public async Task<StatusCodeResult> Delete(string saleTitle)
        {
            var success = await _service.Delete(saleTitle);
            if (success)
                return Ok();
            return BadRequest();
        }
        
        [HttpPost("~/UpdateSale")]
        public async Task<StatusCodeResult> Update(string oldSaleTitle, Sale sale)
        {
            var success = await _service.Update(oldSaleTitle, sale);
            if (success)
                return Ok();
            return BadRequest();
            
        }
        
        [HttpPost("~/AddSaleProducts")]
        public async Task<StatusCodeResult> AddProducts(string saleTitle, HashSet<string> productTitles)
        {
            var success = await _service.AddProducts(saleTitle, productTitles);
            if (success)
                return Ok();
            return BadRequest();
        }
        
        [HttpDelete("~/DeleteSaleProducts")]
        public async Task<StatusCodeResult> DeleteProducts(string saleTitle, HashSet<string> productTitles)
        {
            var success = await _service.DeleteProducts(saleTitle, productTitles);
            if (success)
                return Ok();
            return BadRequest();
        }
        
        [HttpDelete("~/DeleteAllSaleProducts")]
        public async Task<StatusCodeResult> DeleteAllProducts(string saleTitle)
        {
            var success = await _service.DeleteAllProducts(saleTitle);
            if (success)
                return Ok();
            return BadRequest();
        }
    }
}