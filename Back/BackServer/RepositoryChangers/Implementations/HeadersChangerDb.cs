using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using BackServer.RepositoryChangers.Interfaces;
using DbEntity;
using Microsoft.EntityFrameworkCore;

namespace BackServer.RepositoryChangers.Implementations
{
    public class HeadersChangerDb : IHeadersChanger
    {
        private readonly TestContext _context;

        public HeadersChangerDb(TestContext context)
        {
            _context = context;
        }

        public async Task<bool> AddHeadingOne(Entity.HeadingOne headingOne)
        {
            var heading = new HeadingOne() {Title = headingOne.Title};
            await _context.HeadingsOne.AddAsync(heading);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddHeadingTwo(Entity.HeadingTwo headingTwo)
        {
            var headingOne =
                await _context.HeadingsOne.FirstOrDefaultAsync(x => x.Title == headingTwo.HeadingOne.Title);
            if (headingOne == null) return false;

            var newHeadingTwo = new HeadingTwo() {Title = headingTwo.Title, HeadingOne = headingOne};
            await _context.HeadingsTwo.AddAsync(newHeadingTwo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddHeadingThree(Entity.HeadingThree headingThree)
        {
            var headingTwo =
                await _context.HeadingsTwo.FirstOrDefaultAsync(x => x.Title == headingThree.HeadingTwo.Title);
            if (headingTwo == null) return false;

            var property = new Property() {Title = headingThree.Title};
            await _context.Properties.AddAsync(property);
            var newHeadingThree = new HeadingThree() {Property = property, HeadingTwo = headingTwo};
            await _context.HeadingsThree.AddAsync(newHeadingThree);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHeadingOne(string title)
        {
            var heading = await _context.HeadingsOne.FirstOrDefaultAsync(x => x.Title == title);
            if (heading == null)
                return false;

            var products = _context.Products.Where(x => x.HeadingOne == heading).ToHashSet();
            RemoveProducts(products);
            
            
            var headingsTwo = _context.HeadingsTwo.Where(x => x.HeadingOne == heading).ToHashSet();
            var headingsThree =await _context.HeadingsThree.Where(x => headingsTwo.Contains(x.HeadingTwo)).ToArrayAsync();
            _context.HeadingsThree.RemoveRange(headingsThree);
            _context.HeadingsTwo.RemoveRange(headingsTwo);
            
            _context.HeadingsOne.Remove(heading);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHeadingTwo(string title)
        {
            var heading = await _context.HeadingsTwo.FirstOrDefaultAsync(x => x.Title == title);
            if (heading == null)
                return false;
            
            var products = _context.Products.Where(x => x.HeadingTwo == heading).ToHashSet();
            RemoveProducts(products);
            
            var headingsThree =await _context.HeadingsThree.Where(x => x.HeadingTwo == heading).ToArrayAsync();
            _context.HeadingsThree.RemoveRange(headingsThree);

            _context.HeadingsTwo.Remove(heading);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHeadingThree(string title)
        {
            var heading = await _context.HeadingsThree.FirstOrDefaultAsync(x => x.Property.Title == title);
            if (heading == null)
                return false;
            
            var products = _context.Products.Where(x => x.HeadingThree == heading).ToHashSet();
            RemoveProducts(products);

            _context.HeadingsThree.Remove(heading);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHeadingOne(string oldHeadingOneTitle, Entity.HeadingOne headingOne)
        {
            var oldHeadingOne = await _context.HeadingsOne.FirstOrDefaultAsync(x => x.Title == oldHeadingOneTitle);
            if (oldHeadingOne == null)
                return false;

            oldHeadingOne.Title = headingOne.Title;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHeadingTwo(string oldHeadingTwoTitle, Entity.HeadingTwo headingTwo)
        {
            var oldHeadingTwo = await _context.HeadingsTwo.FirstOrDefaultAsync(x => x.Title == oldHeadingTwoTitle);
            if (oldHeadingTwo == null)
                return false;

            oldHeadingTwo.Title = headingTwo.Title;
            if (oldHeadingTwo.HeadingOne.Title != headingTwo.HeadingOne.Title)
            {
                var newHeadingOne =
                    await _context.HeadingsOne.FirstOrDefaultAsync(x => x.Title == headingTwo.HeadingOne.Title);
                if (newHeadingOne == null)
                    return false;

                oldHeadingTwo.HeadingOne = newHeadingOne;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateHeadingThree(string oldHeadingThreeTitle, Entity.HeadingThree headingThree)
        {
            var oldHeadingThree =
                await _context.HeadingsThree.FirstOrDefaultAsync(x => x.Property.Title == oldHeadingThreeTitle);
            if (oldHeadingThree == null)
                return false;

            if (oldHeadingThree.Property.Title != headingThree.Title)
            {
                var newProperty = await _context.Properties.FirstOrDefaultAsync(x => x.Title == headingThree.Title);
                if (newProperty == null) return false;
                oldHeadingThree.Property = newProperty;
            }

            if (oldHeadingThree.HeadingTwo.Title != headingThree.HeadingTwo.Title)
            {
                var newHeadingTwo =
                    await _context.HeadingsTwo.FirstOrDefaultAsync(x => x.Title == headingThree.HeadingTwo.Title);
                if (newHeadingTwo == null)
                    return false;

                oldHeadingThree.HeadingTwo = newHeadingTwo;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private async void RemoveProducts(HashSet<Product> products)
        {
            var sales = await _context.SaleProducts.Where(x => products.Contains(x.Product)).ToArrayAsync();
            var projects = await _context.ProjectMaterials.Where(x => products.Contains(x.Product)).ToArrayAsync();
            var properties = await _context.ProductProperties.Where(x => products.Contains(x.Product)).ToArrayAsync();
            
            _context.SaleProducts.RemoveRange(sales);
            _context.ProjectMaterials.RemoveRange(projects);
            _context.ProductProperties.RemoveRange(properties);
            
            _context.Products.RemoveRange(products);
        }
    }
}