using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace BackServer.Repositories
{
    public interface IProductVisitor
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Entity.Product>> GetAvailable();
        Task<Entity.Product> GetByTitle(string title);
        Task<IEnumerable<Entity.Product>> GetByHeadingOne(string headingOneTitle, HashSet<Property> reqProperties,
            int pageNumber,
            int countElements);
        Task<IEnumerable<Entity.Product>> GetByHeadingTwo(string headingTwoTitle, int pageNumber,
            int countElements);
        Task<IEnumerable<Product>> GetByHeadingThree(string headingTwoTitle, int pageNumber,
            int countElements);
    }
}