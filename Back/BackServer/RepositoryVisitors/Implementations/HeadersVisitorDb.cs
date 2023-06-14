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
                .Select(x=>new Entity.HeadingOne(x.Title))
                .ToListAsync();
        }

        public async Task<IEnumerable<Entity.HeadingTwo>> GetAllHeadingsTwoAsync()
        {
            return await _context.HeadingsTwo
                .Select(x => new Entity.HeadingTwo(x.Title, new Entity.HeadingOne(x.HeadingOne.Title)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Entity.HeadingTwo>> GetHeadingsTwoByHeadingsOneAsync(string headingOneTitle)
        {
            if (headingOneTitle is null or "")
                return Enumerable.Empty<Entity.HeadingTwo>();

            return await _context.HeadingsTwo
                .Where(x => x.HeadingOne.Title == headingOneTitle)
                .Select(x => new Entity.HeadingTwo(x.Title, new Entity.HeadingOne(headingOneTitle)))
                .ToListAsync();
        }
    }
}