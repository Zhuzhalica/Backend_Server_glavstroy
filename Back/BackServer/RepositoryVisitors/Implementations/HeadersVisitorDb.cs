using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using DbEntity;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace BackServer.Repositories
{
    public class HeadersVisitorDb : IHeadersVisitor
    {
        private readonly TestContext _context;

        public HeadersVisitorDb(TestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Entity.HeadingOne>> GetAllHeadingsOneAsync()
        {
            return await _context.HeadingsOne
                .Select(x=>new Entity.HeadingOne(x.Title, x.PageLink))
                .ToListAsync();
        }

        public async Task<IEnumerable<Entity.HeadingTwo>> GetAllHeadingsTwoAsync()
        {
            return await _context.HeadingsTwo
                .Select(x => new Entity.HeadingTwo(x.Title, x.ImageRef, x.PageLink))
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Entity.HeadingThree>> GetAllHeadingsThree()
        {
            return await _context.HeadingsThree
                .Select(x => new Entity.HeadingThree(x.PropertyValues.PropertyValue, x.ImageRef, x.PageLink))
                .ToListAsync();
        }

        public async Task<IEnumerable<Entity.HeadingTwo>> GetHeadingsTwoByHeadingsOneAsync(string headingOneTitle)
        {
            if (headingOneTitle is null or "")
                return Enumerable.Empty<Entity.HeadingTwo>();

            return await _context.HeadingsTwo
                .Where(x => x.HeadingOne.Title == headingOneTitle)
                .Select(x => new Entity.HeadingTwo(x.Title, x.PageLink, x.ImageRef))
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Entity.HeadingThree>> GetHeadingsThreeByHeadingsTwoAsync(string headingTwoTitle)
        {
            if (headingTwoTitle is null or "")
                return Enumerable.Empty<Entity.HeadingThree>();

            return await _context.HeadingsThree
                .Where(x => x.HeadingTwo.Title == headingTwoTitle)
                .Select(x => new Entity.HeadingThree(x.PropertyValues.PropertyValue, x.PageLink, x.ImageRef))
                .ToListAsync();
        }
    }
}