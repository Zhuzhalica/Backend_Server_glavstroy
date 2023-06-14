using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackServer.Repositories
{
    public interface IPropertyVisitor
    {
        Task<IEnumerable<string>> GetAllTitles();
        Task<IEnumerable<Entity.Property>> GetByProduct(string productTitle);
        Task<IEnumerable<Entity.Property>> GetByHeadingTwo(string headingTwoTitle);
    }
}