using System.Threading.Tasks;
using Entity;

namespace BackServer.RepositoryChangers.Interfaces
{
    public interface IPropertyChanger
    {
        Task<bool> Add(Entity.Property property);
        Task<bool> Delete(string propertyTitle);
        Task<bool> Update(Property oldProperty, Property property);
        Task<bool> AddProductPropertyValue(string productTitle, string propertyTitle, string propertyValue);
        Task<bool> DeleteProductPropertyValue(string productTitle, string propertyTitle,
            string propertyValue);
        Task<bool> UpdateProductPropertyValue(string productTitle, string propertyTitle,
            string oldPropertyValue, string newPropertyValue);
        Task<bool> DeleteAllProductProperties(string productTitle);
        Task<bool> DeleteAllPropertyValues(string propertyTitle);
    }
}