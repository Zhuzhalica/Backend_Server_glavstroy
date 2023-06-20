using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace BackServer.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<string>> GetAllTitles();
        Task<IEnumerable<Entity.Property>> GetAllByProduct(string productTitle);
        Task<IEnumerable<Entity.Property>> GetPriorityByProduct(string productTitle);
        Task<IEnumerable<Entity.Property>> GetByHeadingOne(string headingOneTitle);
        Task<IEnumerable<Entity.Property>> GetByHeadingTwo(string headingTwoTitle);
        Task<IEnumerable<Entity.Property>> GetByHeadingThree(string headingThreeTitle);
        Task<bool> Add(Entity.Property property);
        Task<bool> AddProductPropertyValue(string productTitle, string propertyTitle, string propertyValue);
        Task<bool> Delete(string propertyTitle);
        Task<bool> Update(Property oldProperty, Property property);
        Task<bool> DeleteProductPropertyValue(string productTitle, string propertyTitle,
            string propertyValue);
        Task<bool> UpdateProductPropertyValue(string productTitle, string propertyTitle,
            string oldPropertyValue, string newPropertyValue);
        Task<bool> DeleteAllProductProperties(string productTitle);
        Task<bool> DeleteAllPropertyValues(string propertyTitle);
    }
}