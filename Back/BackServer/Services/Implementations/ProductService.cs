using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackServer.Repositories;
using BackServer.RepositoryChangers.Interfaces;
using BackServer.Services.Interfaces;
using Entity;

namespace BackServer.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductVisitor _visitor;
        private readonly IProductChanger _changer;
        private IPropertyService _propertyService;

        public ProductService(IProductVisitor visitor, IProductChanger changer, IPropertyService propertyService)
        {
            _visitor = visitor;
            _changer = changer;
            _propertyService = propertyService;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _visitor.GetAll();
        }

        public async Task<IEnumerable<Product>> GetAvailable()
        {
            return await _visitor.GetAvailable();
        }

        public async Task<IEnumerable<Product>> GetByHeadingOne(string headingOneTitle, HashSet<Property> reqProperties,
            int pageNumber, int countElements)
        {
            if (!CheckCorrectInput(headingOneTitle, pageNumber, countElements))
                return Array.Empty<Product>();

            var products = await _visitor.GetByHeadingOne(headingOneTitle, reqProperties, pageNumber, countElements);
            // foreach (var product in products)
            // {
            //     var props = await _propertyService.GetPriorityByProduct(product.Title);
            //     product.PriorityProperties = props;
            // }

            return products;
        }

        public async Task<IEnumerable<Product>> GetByHeadingTwo(string headingTwoTitle, int pageNumber,
            int countElements)
        {
            if (!CheckCorrectInput(headingTwoTitle, pageNumber, countElements))
                return Array.Empty<Product>();

            return await _visitor.GetByHeadingTwo(headingTwoTitle, pageNumber, countElements);
        }

        public async Task<IEnumerable<Product>> GetByHeadingThree(string headingThreeTitle, int pageNumber,
            int countElements)
        {
            if (!CheckCorrectInput(headingThreeTitle, pageNumber, countElements))
                return Array.Empty<Product>();

            return await _visitor.GetByHeadingThree(headingThreeTitle, pageNumber, countElements);
        }

        public async Task<bool> Add(Product product)
        {
            return await _changer.Add(product);
        }

        public async Task<bool> Delete(HashSet<string> productTitles)
        {
            return await _changer.Delete(productTitles);
        }

        public async Task<bool> Update(string oldProductTitle, Product product)
        {
            return await _changer.Update(oldProductTitle, product);
        }

        public async Task<bool> DeleteHeadingOneProducts(string headingOneTitle)
        {
            return await _changer.DeleteHeadingOneProducts(headingOneTitle);
        }

        public async Task<bool> DeleteHeadingTwoProducts(string headingTwoTitle)
        {
            return await _changer.DeleteHeadingTwoProducts(headingTwoTitle);
        }

        public async Task<bool> DeleteHeadingThreeProducts(string headingThreeTitle)
        {
            return await _changer.DeleteHeadingThreeProducts(headingThreeTitle);
        }

        private bool CheckCorrectInput(string title, int pageNumber, int countElements)
        {
            return title != "" && pageNumber >= 1 && countElements >= 1;
        }
    }
}