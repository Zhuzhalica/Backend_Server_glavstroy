using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace BackServer.Repositories
{
    public interface IProductVisitor
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Entity.Product>> GetByHeadingOne(HeadingOne heading);
        Task<IEnumerable<Entity.Product>> GetByHeadingTwo(HeadingTwo heading);
        Task<IEnumerable<Product>> GetByHeadingThree(HeadingThree heading);
    }
}