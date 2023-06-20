using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using BackServer.RepositoryChangers.Interfaces;
using DbEntity;
using Microsoft.EntityFrameworkCore;
using Sale = Entity.Sale;

namespace BackServer.RepositoryChangers.Implementations
{
    public class SaleChangerDb : ISaleChanger
    {
        private readonly TestContext _context;

        public SaleChangerDb(TestContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Sale sale)
        {
            var saleDb = new DbEntity.Sale()
                {Title = sale.Title, Description = sale.Description, Percent = sale.Percent, PageLink = sale.PageLink, ImageRef = sale.ImageRef};

            await _context.Sales.AddAsync(saleDb);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(string saleTitle)
        {
            var sale = await _context.Sales.FirstOrDefaultAsync(x => x.Title == saleTitle);
            if (sale == null)
                return false;

            var products = await _context.SaleProducts.Where(x => x.Sale.Title == saleTitle).ToArrayAsync();
            _context.SaleProducts.RemoveRange(products);
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(string oldSaleTitle, Sale sale)
        {
            var oldSale = await _context.Sales.FirstOrDefaultAsync(x => x.Title == oldSaleTitle);
            if (oldSale == null)
                return false;

            oldSale.Title = sale.Title;
            oldSale.Description = sale.Description;
            oldSale.Percent = sale.Percent;
            oldSale.PageLink = sale.PageLink;
            oldSale.ImageRef = sale.ImageRef;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddProducts(string saleTitle, HashSet<string> productTitles)
        {
            var sale = await _context.Sales.FirstOrDefaultAsync(x => x.Title == saleTitle);
            if (sale == null)
                return false;

            var saleProducts = await _context.Products
                .Where(x => productTitles.Contains(x.Title))
                .Select(x => new SaleProducts() {Sale = sale, Product = x})
                .ToArrayAsync();
            if (saleProducts.Length == 0)
                return false;

            await _context.SaleProducts.AddRangeAsync(saleProducts);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProducts(string saleTitle, HashSet<string> productTitles)
        {
            var saleProducts = await _context.SaleProducts
                .Where(x => x.Sale.Title==saleTitle && productTitles.Contains(x.Product.Title))
                .ToArrayAsync();
            if (saleProducts.Length == 0)
                return false;

            _context.SaleProducts.RemoveRange(saleProducts);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllProducts(string saleTitle)
        {
            var sale = await _context.Sales.FirstOrDefaultAsync(x => x.Title == saleTitle);
            if (sale == null)
                return false;

            var saleProducts = await _context.SaleProducts
                .Where(x => x.Sale == sale)
                .ToArrayAsync();
            if (saleProducts.Length == 0)
                return false;

            _context.SaleProducts.RemoveRange(saleProducts);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}