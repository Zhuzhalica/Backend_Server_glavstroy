using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackServer.RepositoryChangers.Interfaces
{
    public interface IProjectChanger
    {
        Task<bool> Add(Entity.Project project);
        Task<bool> Delete(string projectTitle);
        Task<bool> Update(string oldProjectTitle, Entity.Project project);
        Task<bool> AddProducts(string projectTitle, HashSet<string> productTitles);
        Task<bool> DeleteProducts(string projectTitle, HashSet<string> productTitles);
        Task<bool> DeleteAllProduct(string projectTitle);
    }
}