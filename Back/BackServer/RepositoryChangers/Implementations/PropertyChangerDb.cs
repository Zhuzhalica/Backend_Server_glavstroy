using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using BackServer.RepositoryChangers.Interfaces;
using DbEntity;
using Microsoft.EntityFrameworkCore;
using Property = Entity.Property;

namespace BackServer.RepositoryChangers.Implementations
{
    public class PropertyChangerDb: IPropertyChanger
    {
        private readonly TestContext _context;

        public PropertyChangerDb(TestContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Property property)
        {
            var propertyDb = new DbEntity.Property() {Title = property.Title};
            await _context.Properties.AddAsync(propertyDb);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> Delete(string propertyTitle)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(x => x.Title == propertyTitle);
            if (property == null)
                return false;

            var productProperties = await _context.ProductProperties.Where(x => x.Property == property).ToArrayAsync();
            _context.ProductProperties.RemoveRange(productProperties);
            
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(string oldPropertyTitle, Property property)
        {
            var propertyDb = await _context.Properties.FirstOrDefaultAsync(x => x.Title == oldPropertyTitle);
            if (propertyDb == null)
                return false;

            propertyDb.Title = property.Title;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> AddProductPropertyValue(string productTitle, string propertyTitle, string propertyValue)
        {
            var property = await _context.Properties.FirstOrDefaultAsync(x => x.Title == propertyTitle);
            if (property == null)
                return false;
            
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Title == productTitle);
            if (product == null)
                return false;
            
            await _context.ProductProperties.AddAsync(new ProductProperty()
                {Property = property, Product = product, PropertyValue = propertyValue});
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductPropertyValue(string productTitle, string propertyTitle, string propertyValue)
        {
            var productProperty = await _context.ProductProperties.FirstOrDefaultAsync(x =>
                x.Product.Title == productTitle && x.Property.Title == propertyTitle &&
                x.PropertyValue == propertyValue);
            if (productProperty == null)
                return false;

            _context.ProductProperties.Remove(productProperty);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> UpdateProductPropertyValue(string productTitle, string propertyTitle, string oldPropertyValue, string newPropertyValue)
        {
            var productProperty = await _context.ProductProperties.FirstOrDefaultAsync(x =>
                x.Product.Title == productTitle && x.Property.Title == propertyTitle &&
                x.PropertyValue == oldPropertyValue);
            if (productProperty == null)
                return false;

            productProperty.PropertyValue = newPropertyValue;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> DeleteAllProductProperties(string productTitle)
        {
            var productProperties = await _context.ProductProperties.Where(x =>
                x.Product.Title == productTitle).ToArrayAsync();
            if (productProperties.Length == 0)
                return false;
            
            _context.ProductProperties.RemoveRange(productProperties);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> DeleteAllValuesProductProperty(string productTitle, string propertyTitle)
        {
            var productProperties = await _context.ProductProperties.Where(x =>
                x.Product.Title == productTitle && x.Property.Title==propertyTitle).ToArrayAsync();
            if (productProperties.Length == 0)
                return false;
            
            _context.ProductProperties.RemoveRange(productProperties);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}