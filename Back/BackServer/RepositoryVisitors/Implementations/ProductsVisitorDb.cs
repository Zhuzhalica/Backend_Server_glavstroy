using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using DbEntity;
using DbEntityConverter;
using Entity;
using Microsoft.EntityFrameworkCore;
using HeadingThree = Entity.HeadingThree;
using Product = Entity.Product;

namespace BackServer.Repositories
{
    public class ProductsVisitorDb : IProductVisitor
    {
        private readonly TestContext _context;

        public ProductsVisitorDb(TestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entity.Product>> GetAll()
        {
            return await _context.Products.Select(x=>ProductConverter.ToEntity(x))
                .ToListAsync();
        }

        public async Task<IEnumerable<Entity.Product>> GetByHeadingOne(Entity.HeadingOne heading)
        {
            // return await _context.Products
            //     .Where(x => x.HeadingOne == heading)
            //     .Join();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Entity.Product>> GetByHeadingTwo(Entity.HeadingTwo heading)
        {
            // return await _context.Products
            //     .Where(x => x.HeadingTwo == heading)
            //     .Select(x => new Entity.Product()
            //         {HeadingOne = x.HeadingOne, HeadingTwo = x.HeadingTwo});
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetByHeadingThree(HeadingThree heading)
        {
            throw new NotImplementedException();
        }
    }
}