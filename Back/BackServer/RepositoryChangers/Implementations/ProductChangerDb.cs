using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using BackServer.RepositoryChangers.Interfaces;
using DbEntity;
using Microsoft.EntityFrameworkCore;
using Product = Entity.Product;

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
            var headingOne = await _context.HeadingsOne.FirstOrDefaultAsync(x => x.Title == product.HeadingOne);
            var headingTwo = await _context.HeadingsTwo.FirstOrDefaultAsync(x => x.Title == product.HeadingTwo);
            var unitMeasurement =
                await _context.UnitMeasurements.FirstOrDefaultAsync(x => x.Value == product.UnitMeasurement);
            if (headingOne == null || headingTwo == null || unitMeasurement == null) return false;

            var productFamily =
                await _context.ProductFamilies.FirstOrDefaultAsync(x => x.Title == product.ProductFamilyTitle);
            if (productFamily == null)
            {
                productFamily = new ProductFamily()
                {
                    Title = product.ProductFamilyTitle, HeadingOne = headingOne,
                    HeadingTwo = headingTwo, UnitMeasurement = unitMeasurement
                };
                await _context.ProductFamilies.AddAsync(productFamily);
            }
            

            DbEntity.HeadingThree? headingThree = null;
            if (product.HeadingThree != null)
            {
                headingThree = await _context.HeadingsThree
                    .FirstOrDefaultAsync(x => x.PropertyValues.PropertyValue == product.HeadingThree);
                if (headingThree == null) return false;
            }

            await _context.Products.AddAsync(ToDbEntity(product, headingThree, productFamily));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(HashSet<string> productTitles)
        {
            var products = _context.Products.Where(x => productTitles.Contains(x.Title)).ToHashSet();
            if (products.Count == 0)
                return false;

            await RemoveProductsLinks(products);

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(string oldProductTitle, Product product)
        {
            var oldProduct = await _context.Products.FirstOrDefaultAsync(x => x.Title == oldProductTitle);
            if (oldProduct == null)
                return false;
            var oldProductFamily =
                await _context.ProductFamilies.FirstAsync(x => x.Id == oldProduct.product_family_id);

            oldProduct.Title = product.Title;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;
            oldProduct.Quantity = product.Quantity;
            oldProduct.Popularity = product.Popularity;
            oldProduct.Available = product.Available;
            oldProduct.ImageRef = product.ImageRef;
            oldProduct.PageLink = product.PageLink;

            if (oldProductFamily.Title != product.Title)
            {
                var productFamily = await _context.ProductFamilies.FirstOrDefaultAsync(x => x.Title == product.Title);
                if (productFamily == null)
                    return false;
                oldProductFamily = productFamily;
                oldProduct.ProductFamily = oldProductFamily;
            }

            if (oldProductFamily.UnitMeasurement.Value != product.UnitMeasurement)
            {
                var unitMeasurement = await _context.UnitMeasurements.FirstOrDefaultAsync(x => x.Value == product.UnitMeasurement);
                if (unitMeasurement == null)
                    return false;
                oldProductFamily.UnitMeasurement= unitMeasurement;
            }
            
            if (oldProductFamily.HeadingOne.Title != product.HeadingOne)
            {
                var heading = await _context.HeadingsOne.FirstOrDefaultAsync(x => x.Title == product.HeadingOne);
                if (heading == null)
                    return false;
                oldProductFamily.HeadingOne = heading;
            }

            if (oldProductFamily.HeadingTwo.Title != product.HeadingTwo)
            {
                var heading = await _context.HeadingsTwo.FirstOrDefaultAsync(x => x.Title == product.HeadingTwo);
                if (heading == null)
                    return false;
                oldProductFamily.HeadingTwo = heading;
            }

            if (oldProduct.HeadingThree!.PropertyValues.PropertyValue != product.HeadingThree!)
            {
                var heading = await _context.HeadingsThree.FirstOrDefaultAsync(x =>
                    x.PropertyValues.PropertyValue == product.HeadingThree);
                if (heading == null)
                    return false;
                oldProduct.HeadingThree = heading;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHeadingOneProducts(string headingOneTitle)
        {
            var headingOne = await _context.HeadingsOne.FirstOrDefaultAsync(x => x.Title == headingOneTitle);
            if (headingOne == null)
                return false;

            var productFamilyIds = _context.ProductFamilies.Where(x => x.HeadingOne == headingOne).Select(x => x.Id)
                .ToHashSet();
            var products = _context.Products.Where(x => productFamilyIds.Contains(x.product_family_id)).ToHashSet();
            await RemoveProductsLinks(products);

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHeadingTwoProducts(string headingTwoTitle)
        {
            var headingTwo = await _context.HeadingsTwo.FirstOrDefaultAsync(x => x.Title == headingTwoTitle);
            if (headingTwo == null)
                return false;

            var productFamilyIds = _context.ProductFamilies.Where(x => x.HeadingTwo == headingTwo).Select(x => x.Id)
                .ToHashSet();
            var products = _context.Products.Where(x => productFamilyIds.Contains(x.product_family_id)).ToHashSet();
            await RemoveProductsLinks(products);

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHeadingThreeProducts(string headingThreeTitle)
        {
            var headingThree =
                await _context.HeadingsThree.FirstOrDefaultAsync(x =>
                    x.PropertyValues.PropertyValue == headingThreeTitle);
            if (headingThree == null)
                return false;

            var products = _context.Products.Where(x => x.HeadingThree == headingThree).ToHashSet();
            await RemoveProductsLinks(products);

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task RemoveProductsLinks(IReadOnlySet<DbEntity.Product> products)
        {
            var saleProductsArray = await _context.SaleProducts.Where(x => products.Contains(x.Product)).ToArrayAsync();
            var projectMaterialsArray =
                await _context.ProjectMaterials.Where(x => products.Contains(x.Product)).ToArrayAsync();
            var productProperties =
                await _context.ProductProperties.Where(x => products.Contains(x.Product)).ToArrayAsync();

            _context.SaleProducts.RemoveRange(saleProductsArray);
            _context.ProjectMaterials.RemoveRange(projectMaterialsArray);
            _context.ProductProperties.RemoveRange(productProperties);
        }

        private static DbEntity.Product ToDbEntity(Product product, DbEntity.HeadingThree? headingThree, ProductFamily productFamily)
        {
            return new DbEntity.Product()
            {
                Title = product.Title, Description = product.Description, Price = product.Price,
                Quantity = product.Quantity, Popularity = product.Popularity, Available = product.Available,
                ImageRef = product.ImageRef, PageLink = product.PageLink,
                HeadingThree = headingThree, ProductFamily = productFamily
            };
        }
    }
}