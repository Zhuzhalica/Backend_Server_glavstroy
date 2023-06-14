using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using DbEntityConverter;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace BackServer.Repositories
{
    public class SalesVisitorDb : ISaleVisitor
    {
        private readonly TestContext _context;

        public SalesVisitorDb(TestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entity.Sale>> GetAllSales()
        {
            return await _context.Sales.Select(x => new Sale(x.Title, x.Description, x.Percent)).ToArrayAsync();
        }
        
        public async Task<IEnumerable<Entity.Product>> GetProductsBySale(string saleTitle)
        {
            return await _context.SaleProducts
                .Join(_context.Sales, cur => cur.sale_id, other => other.Id,
                    (cur, other) => new {other.Title, ProductId = cur.product_id})
                .Where(x=>x.Title==saleTitle)
                .Join(_context.Products, cur => cur.ProductId, other=>other.Id,
                    (cur, other)=> ProductConverter.ToEntity(other))
                .ToArrayAsync();
        }
    }
}