using System.Threading.Tasks;

namespace BackServer.RepositoryChangers.Interfaces
{
    public interface IPropertyChanger
    {
        Task<bool> Add(Entity.Property property);
        Task<bool> Delete(string propertyTitle);
        Task<bool> Update(string oldPropertyTitle, Entity.Property property);
        Task<bool> AddProductPropertyValue(string productTitle, string propertyTitle, string propertyValue);
        Task<bool> DeleteProductPropertyValue(string productTitle, string propertyTitle,
            string propertyValue);
        Task<bool> UpdateProductPropertyValue(string productTitle, string propertyTitle,
            string oldPropertyValue, string newPropertyValue);
        Task<bool> DeleteAllProductProperties(string productTitle);
        Task<bool> DeleteAllValuesProductProperty(string productTitle, string propertyTitle);
    }
}