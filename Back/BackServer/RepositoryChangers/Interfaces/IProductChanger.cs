using System.Threading.Tasks;
using Entity;

namespace BackServer.RepositoryChangers.Interfaces
{
    public interface IProductChanger
    {
        Task<bool> Add(Product product);
        Task<bool> Delete(string productTitle);
        Task<bool> Update(string oldProductTitle, Product product);
    }
}