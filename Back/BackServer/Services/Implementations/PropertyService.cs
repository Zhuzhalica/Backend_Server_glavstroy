using System.Collections.Generic;
using System.Threading.Tasks;
using BackServer.Repositories;
using BackServer.RepositoryChangers.Interfaces;
using BackServer.Services.Interfaces;
using Entity;

namespace BackServer.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyVisitor _visitor;
        private readonly IPropertyChanger _changer;

        public PropertyService(IPropertyVisitor visitor, IPropertyChanger changer)
        {
            _visitor = visitor;
            _changer = changer;
        }

        public async Task<IEnumerable<string>> GetAllTitles()
        {
            return await _visitor.GetAllTitles();
        }

        public async Task<IEnumerable<Property>> GetAllByProduct(string productTitle)
        {
            return await _visitor.GetAllByProduct(productTitle);
        }

        public async Task<IEnumerable<Property>> GetPriorityByProduct(string productTitle)
        {
            return await _visitor.GetPriorityByProduct(productTitle);
        }

        public async Task<IEnumerable<Property>> GetByHeadingOne(string headingOneTitle)
        {
            return await _visitor.GetByHeadingOne(headingOneTitle);
        }

        public async Task<IEnumerable<Property>> GetByHeadingTwo(string headingTwoTitle)
        {
            return await _visitor.GetByHeadingTwo(headingTwoTitle);
        }

        public async Task<IEnumerable<Property>> GetByHeadingThree(string headingThreeTitle)
        {
            return await _visitor.GetByHeadingThree(headingThreeTitle);
        }

        public async Task<bool> Add(Property property)
        {
            return await _changer.Add(property);
        }

        public async Task<bool> AddProductPropertyValue(string productTitle, string propertyTitle, string propertyValue)
        {
            return await _changer.AddProductPropertyValue(productTitle, propertyTitle, propertyValue);
        }

        public async Task<bool> Delete(string propertyTitle)
        {
            return await _changer.Delete(propertyTitle);
        }

        public async Task<bool> Update(Property oldProperty, Property property)
        {
            return await _changer.Update(oldProperty, property);
        }

        public async Task<bool> DeleteProductPropertyValue(string productTitle, string propertyTitle, string propertyValue)
        {
            return await _changer.DeleteProductPropertyValue(productTitle, propertyTitle, propertyValue);
        }

        public async Task<bool> UpdateProductPropertyValue(string productTitle, string propertyTitle, string oldPropertyValue,
            string newPropertyValue)
        {
            return await _changer.UpdateProductPropertyValue(productTitle, propertyTitle, oldPropertyValue,
                newPropertyValue);
        }

        public async Task<bool> DeleteAllProductProperties(string productTitle)
        {
            return await _changer.DeleteAllProductProperties(productTitle);
        }

        public async Task<bool> DeleteAllPropertyValues(string propertyTitle)
        {
            return await _changer.DeleteAllPropertyValues(propertyTitle);
        }
    }
}