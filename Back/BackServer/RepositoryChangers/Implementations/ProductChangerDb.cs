using System;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using BackServer.RepositoryChangers.Interfaces;
using DbEntityConverter;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace BackServer.RepositoryChangers.Implementations
{
    public class ProductChangerDb : IProductChanger
    {
        private readonly TestContext _context;

        public ProductChangerDb(TestContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Product product)
        {
            var headingOne = await _context.HeadingsOne.FirstOrDefaultAsync(x => x.Title == product.HeadingOne.Title);
            if (headingOne == null) return false;

            var headingTwo = await _context.HeadingsTwo.FirstOrDefaultAsync(x => x.Title == product.HeadingTwo.Title);
            if (headingTwo == null) return false;

            var headingThree = await _context.HeadingsThree
                .FirstOrDefaultAsync(x => x.Property.Title == product.HeadingThree.Title);
            if (headingThree == null) return false;

            await _context.Products.AddAsync(ProductConverter.ToDbEntity(product, headingOne, headingTwo,
                headingThree));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(string productTitle)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Title == productTitle);
            if (product == null)
                return false;

            var sales =await _context.SaleProducts.Where(x => x.Product == product).ToArrayAsync();
            var projects = await _context.ProjectMaterials.Where(x => x.Product == product).ToArrayAsync();
            var properties = await _context.ProductProperties.Where(x => x.Product == product).ToArrayAsync();

            _context.SaleProducts.RemoveRange(sales);
            _context.ProjectMaterials.RemoveRange(projects);
            _context.ProductProperties.RemoveRange(properties);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(string oldProductTitle, Product product)
        {
            var oldProduct = await _context.Products.FirstOrDefaultAsync(x => x.Title == oldProductTitle);
            if (oldProduct == null)
                return false;

            oldProduct.Title = product.Title;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;
            oldProduct.Quantity = product.Quantity;

            if (oldProduct.HeadingOne.Title != product.HeadingOne.Title)
            {
                var heading = await _context.HeadingsOne.FirstOrDefaultAsync(x => x.Title == product.HeadingOne.Title);
                if (heading == null)
                    return false;
                oldProduct.HeadingOne = heading;
            }
            
            if (oldProduct.HeadingTwo.Title != product.HeadingTwo.Title)
            {
                var heading = await _context.HeadingsTwo.FirstOrDefaultAsync(x => x.Title == product.HeadingTwo.Title);
                if (heading == null)
                    return false;
                oldProduct.HeadingTwo = heading;
            }
            
            if (oldProduct.HeadingThree.Property.Title != product.HeadingThree.Title)
            {
                var heading = await _context.HeadingsThree.FirstOrDefaultAsync(x => x.Property.Title == product.HeadingThree.Title);
                if (heading == null)
                    return false;
                oldProduct.HeadingThree = heading;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}