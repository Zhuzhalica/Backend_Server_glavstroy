using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackServer.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<string>> GetAllTitles();
        Task<IEnumerable<Entity.Property>> GetByProduct(string productTitle);
        Task<IEnumerable<Entity.Property>> GetByHeadingTwo(string headingTwoTitle);
        Task<bool> Add(Entity.Property property);
        Task<bool> AddProductPropertyValue(string productTitle, string propertyTitle, string propertyValue);
        Task<bool> Delete(string propertyTitle);
        Task<bool> Update(string oldPropertyTitle, Entity.Property property);
        Task<bool> DeleteProductPropertyValue(string productTitle, string propertyTitle,
            string propertyValue);
        Task<bool> UpdateProductPropertyValue(string productTitle, string propertyTitle,
            string oldPropertyValue, string newPropertyValue);
        Task<bool> DeleteAllProductProperties(string productTitle);
        Task<bool> DeleteAllValuesProductProperty(string productTitle, string propertyTitle);
    }
}