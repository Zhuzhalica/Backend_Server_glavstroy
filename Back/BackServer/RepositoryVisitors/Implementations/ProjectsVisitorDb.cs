using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using DbEntityConverter;
using Entity;
using Microsoft.EntityFrameworkCore;
using Project = DbEntity.Project;

namespace BackServer.Repositories
{
    public class ProjectsVisitorDb : IProjectVisitor
    {
        private readonly TestContext _context;

        public ProjectsVisitorDb(TestContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Entity.Project>> GetAll()
        {
            return await _context.Projects.Select(x=>new Entity.Project(x.Title, x.RoofType)).ToListAsync();
        }
        
        public async Task<IEnumerable<Entity.Project>> GetRange(int left, int right)
        {
            return await _context.Projects.Select(x=>new Entity.Project(x.Title, x.RoofType)).Take(right).Skip(left).ToListAsync();
        }
        
        public async Task<IEnumerable<Entity.Product>> GetProductByProject(string projectTitle)
        {
            return await _context.ProjectMaterials
                .Join(_context.Projects, cur => cur.project_id, other => other.Id,
                    (cur, other) => new {ProductId = cur.product_id, other.Title})
                .Where(x => x.Title == projectTitle)
                .Join(_context.Products, cur => cur.ProductId, other => other.Id,
                    (cur, other) => ProductConverter.ToEntity(other))
                .ToArrayAsync();
        }

        public bool Add(Project project)
        {
            // try
            // {
            //     using (TestContext db = new())
            //     {
            //         db.Projects.Add(project);
            //         db.SaveChanges();
            //         return true;
            //     }
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     return false;
            // }
            throw new NotImplementedException();
        }
        
        public bool Delete(Project project)
        {
            // try
            // {
            //     using (TestContext db = new())
            //     {
            //         db.Projects.Remove(project);
            //         db.SaveChanges();
            //         return true;
            //     }
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     return false;
            // }
            throw new NotImplementedException();
        }
    }
}