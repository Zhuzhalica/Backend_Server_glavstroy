using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace BackServer.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Entity.Product>> GetAvailable();

        Task<IEnumerable<Entity.Product>> GetByHeadingOne(string headingOneTitle, HashSet<Property> reqProperties,
            int pageNumber, int countElements);
        Task<IEnumerable<Entity.Product>> GetByHeadingTwo(string headingTwoTitle, int pageNumber,
            int countElements);
        Task<IEnumerable<Product>> GetByHeadingThree(string headingThreeTitle, int pageNumber,
            int countElements);
        Task<bool> Add(Product product);
        Task<bool> Delete(HashSet<string> productTitles);
        Task<bool> Update(string oldProductTitle, Product product);
        Task<bool> DeleteHeadingOneProducts(string headingOneTitle);
        Task<bool> DeleteHeadingTwoProducts(string headingTwoTitle);
        Task<bool> DeleteHeadingThreeProducts(string headingThreeTitle);
    }
}