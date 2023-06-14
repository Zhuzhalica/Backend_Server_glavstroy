using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace BackServer.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Entity.Product>> GetByHeadingOne(HeadingOne heading);
        Task<IEnumerable<Entity.Product>> GetByHeadingTwo(HeadingTwo heading);
        Task<IEnumerable<Product>> GetByHeadingThree(HeadingThree heading);
        Task<bool> Add(Product product);
        Task<bool> Delete(string productTitle);
        Task<bool> Update(string oldProductTitle, Product product);
    }
}