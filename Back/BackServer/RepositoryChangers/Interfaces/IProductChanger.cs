using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace BackServer.RepositoryChangers.Interfaces
{
    public interface IProductChanger
    {
        Task<bool> Add(Product product);
        Task<bool> Delete(HashSet<string> productTitles);
        Task<bool> Update(string oldProductTitle, Product product);
        Task<bool> DeleteHeadingOneProducts(string headingOneTitle);
        Task<bool> DeleteHeadingTwoProducts(string headingTwoTitle);
        Task<bool> DeleteHeadingThreeProducts(string headingThreeTitle);
    }
}