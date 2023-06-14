using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackServer.RepositoryChangers.Interfaces
{
    public interface ISaleChanger
    {
        Task<bool> Add(Entity.Sale sale);
        Task<bool> Delete(string saleTitle);
        Task<bool> Update(string oldSaleTitle, Entity.Sale sale);
        Task<bool> AddProducts(string saleTitle, HashSet<string> productTitles);
        Task<bool> DeleteProducts(string saleTitle, HashSet<string> productTitles);
        Task<bool> DeleteAllProducts(string saleTitle);
    }
}