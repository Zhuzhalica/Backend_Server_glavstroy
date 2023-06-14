using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using DbEntity;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace BackServer.Repositories
{
    public class PropertiesVisitorDb : IPropertyVisitor
    {
        private readonly TestContext _context;

        public PropertiesVisitorDb(TestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetAllTitles()
        {
            return await _context.Properties.Select(x => x.Title).ToArrayAsync();
        }

        public async Task<IEnumerable<Entity.Property>> GetByProduct(string productTitle)
        {
            return await _context.ProductProperties
                .Join(_context.Products, cur => cur.product_id, other => other.Id,
                    (cur, other) => new {cur.property_id, other.Title, cur.PropertyValue})
                .Where(x => x.Title == productTitle)
                .Join(_context.Properties, cur => cur.property_id, other => other.Id,
                    (cur, other) => new Entity.Property(other.Title, cur.PropertyValue))
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Entity.Property>> GetByHeadingTwo(string headingTwoTitle)
        {
            return await _context.ProductProperties
                .Join(_context.Products, cur => cur.product_id, other => other.Id,
                    (cur, other) => new
                    {
                        other.HeadingTwo, PropertyId = cur.property_id, cur.PropertyValue
                    })
                .Join(_context.HeadingsTwo, cur => cur.HeadingTwo.Id, other => other.Id,
                    (cur, other) => new {cur.PropertyId, cur.PropertyValue, HeadingTwoTitle = other.Title})
                .Where(x => x.HeadingTwoTitle == headingTwoTitle)
                .Join(_context.Properties, cur => cur.PropertyId, other => other.Id, (cur, other) =>
                    new Entity.Property(other.Title, cur.PropertyValue))
                .ToArrayAsync();
        }
    }
}