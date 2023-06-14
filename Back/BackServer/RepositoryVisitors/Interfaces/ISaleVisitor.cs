using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackServer.Repositories
{
    public interface ISaleVisitor
    {
        Task<IEnumerable<Entity.Sale>> GetAllSales();
        Task<IEnumerable<Entity.Product>> GetProductsBySale(string saleTitle);
    }
}