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

        public ProductService(IProductVisitor visitor, IProductChanger changer)
        {
            _visitor = visitor;
            _changer = changer;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _visitor.GetAll();
        }

        public async Task<IEnumerable<Product>> GetByHeadingOne(HeadingOne heading)
        {
            if (heading.Title == null)
                return Array.Empty<Product>();

            return await _visitor.GetByHeadingOne(heading);
        }

        public async Task<IEnumerable<Product>> GetByHeadingTwo(HeadingTwo heading)
        {
            if (heading.Title == null)
                return Array.Empty<Product>();

            return await _visitor.GetByHeadingTwo(heading);
        }

        public async Task<IEnumerable<Product>> GetByHeadingThree(HeadingThree heading)
        {
            if (heading.Title == null)
                return Array.Empty<Product>();

            return await _visitor.GetByHeadingThree(heading);
        }

        public async Task<bool> Add(Product product)
        {
            if (!CheckCorrectProduct(product))
                return false;
            return await _changer.Add(product);
        }

        public async Task<bool> Delete(string productTitle)
        {
            return await _changer.Delete(productTitle);
        }

        public async Task<bool> Update(string oldProductTitle, Product product)
        {
            if (!CheckCorrectProduct(product))
                return false;
            return await _changer.Update(oldProductTitle, product);
        }

        public bool CheckCorrectProduct(Product product)
        {
            return product.Title != null && product.Description != null && product.Price != null &&
                   product.Quantity != null && product.HeadingOne != null && product.HeadingTwo != null &&
                   product.HeadingThree != null;
        }
    }
}