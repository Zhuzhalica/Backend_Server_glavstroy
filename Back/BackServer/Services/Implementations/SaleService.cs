using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Repositories;
using BackServer.RepositoryChangers.Interfaces;
using BackServer.Services.Interfaces;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace BackServer.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleVisitor _visitor;
        private readonly ISaleChanger _changer;

        public SaleService(ISaleVisitor visitor, ISaleChanger changer)
        {
            _visitor = visitor;
            _changer = changer;
        }

        public async Task<IEnumerable<Sale>> GetAllSales()
        {
            return await _visitor.GetAllSales();
        }

        public async Task<IEnumerable<Product>> GetProductsBySale(string saleTitle)
        {
            return await _visitor.GetProductsBySale(saleTitle);
        }

        public async Task<bool> Add(Sale sale)
        {
            if (!CheckSaleCorrect(sale))
                return false;
            
            return await _changer.Add(sale);
        }

        public async Task<bool> Delete(string saleTitle)
        {
            return await _changer.Delete(saleTitle);
        }

        public async Task<bool> Update(string oldSaleTitle, Sale sale)
        {
            if (!CheckSaleCorrect(sale))
                return false;
            
            return await _changer.Update(oldSaleTitle, sale);
        }

        public async Task<bool> AddProducts(string saleTitle, HashSet<string> productTitles)
        {
            return await _changer.AddProducts(saleTitle, productTitles);
        }

        public async Task<bool> DeleteProducts(string saleTitle, HashSet<string> productTitles)
        {
            return await _changer.DeleteProducts(saleTitle, productTitles);
        }

        public async Task<bool> DeleteAllProducts(string saleTitle)
        {
            return await _changer.DeleteAllProducts(saleTitle);
        }

        private bool CheckSaleCorrect(Sale sale)
        {
            return sale.Title != null && sale.Percent != null && sale.Description != null;
        }
    }
}