using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackServer.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Entity.Project>> GetAll();
        Task<IEnumerable<Entity.Project>> GetRange(int left, int right);
        Task<IEnumerable<Entity.Product>> GetProductByProject(string projectTitle);
        Task<bool> Add(Entity.Project project);
        Task<bool> Delete(string projectTitle);
        Task<bool> Update(string oldProjectTitle, Entity.Project project);
        Task<bool> AddProducts(string projectTitle, HashSet<string> productTitles);
        Task<bool> DeleteProducts(string projectTitle, HashSet<string> productTitles);
        Task<bool> DeleteAllProduct(string projectTitle);
    }
}