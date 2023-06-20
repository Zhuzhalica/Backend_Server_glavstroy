using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackServer.Repositories
{
    public interface IProjectVisitor
    {
        Task<IEnumerable<Entity.Project>> GetAll();
        Task<IEnumerable<Entity.Project>> GetRange(int pageNumber, int countElements);
        Task<IEnumerable<Entity.Product>> GetProductByProject(string projectTitle);
        Task<IEnumerable<Entity.Project>> GetProjectByProduct(string productTitle);
    }
}