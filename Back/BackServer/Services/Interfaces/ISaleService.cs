using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackServer.Services.Interfaces
{
    public interface ISaleService
    {
        Task<IEnumerable<Entity.Sale>> GetAllSales();
        Task<IEnumerable<Entity.Product>> GetProductsBySale(string saleTitle);
        Task<bool> Add(Entity.Sale sale);
        Task<bool> Delete(string saleTitle);
        Task<bool> Update(string oldSaleTitle, Entity.Sale sale);
        Task<bool> AddProducts(string saleTitle, HashSet<string> productTitles);
        Task<bool> DeleteProducts(string saleTitle, HashSet<string> productTitles);
        Task<bool> DeleteAllProducts(string saleTitle);
    }
}