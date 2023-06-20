using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackServer.Contexts;
using BackServer.RepositoryChangers.Interfaces;
using DbEntity;
using Microsoft.EntityFrameworkCore;
using Project = Entity.Project;

namespace BackServer.RepositoryChangers.Implementations
{
    public class ProjectChangerDb : IProjectChanger
    {
        private readonly TestContext _context;

        public ProjectChangerDb(TestContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Entity.Project project)
        {
            await _context.Projects.AddAsync(new DbEntity.Project(project.Title, project.RoofType));
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(string projectTitle)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Title == projectTitle);
            if (project == null)
                return false;

            var projectMaterials = await _context.ProjectMaterials.Where(x => x.Project.Title == projectTitle).ToArrayAsync();
            _context.ProjectMaterials.RemoveRange(projectMaterials);
            
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(string oldProjectTitle, Project project)
        {
            var oldProject = await _context.Projects.FirstOrDefaultAsync(x => x.Title == oldProjectTitle);
            if (oldProject == null)
                return false;

            oldProject.Title = project.Title;
            oldProject.RoofType = project.RoofType;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddProducts(string projectTitle, HashSet<string> productTitles)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Title == projectTitle);
            if (project == null)
                return false;
            var projectMaterials = await _context.Products.Where(x => productTitles.Contains(x.Title))
                .Select(x => new ProjectMaterials() {Project = project, Product = x}).ToArrayAsync();

            await _context.ProjectMaterials.AddRangeAsync(projectMaterials);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProducts(string projectTitle, HashSet<string> productTitles)
        {
            var projectMaterials = await _context.ProjectMaterials.FirstOrDefaultAsync(x =>
                productTitles.Contains(x.Product.Title) && x.Project.Title == projectTitle);
            if (projectMaterials == null)
                return false;


            _context.ProjectMaterials.Remove(projectMaterials);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllProduct(string projectTitle)
        {
            var projectMaterials =
                await _context.ProjectMaterials.Where(x => x.Project.Title == projectTitle).ToArrayAsync();
            if (projectMaterials.Length == 0)
                return false;


            _context.ProjectMaterials.RemoveRange(projectMaterials);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}