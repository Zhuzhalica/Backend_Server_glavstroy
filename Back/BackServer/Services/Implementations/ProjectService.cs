using System.Collections.Generic;
using System.Threading.Tasks;
using BackServer.Repositories;
using BackServer.RepositoryChangers.Interfaces;
using BackServer.Services.Interfaces;
using DbEntity;
using Project = Entity.Project;

namespace BackServer.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectVisitor _visitor;
        private readonly IProjectChanger _changer;

        public ProjectService(IProjectVisitor visitor, IProjectChanger changer)
        {
            _visitor = visitor;
            _changer = changer;
        }

        public async Task<IEnumerable<Entity.Project>> GetAll()
        {
            return await _visitor.GetAll();
        }

        public async Task<IEnumerable<Entity.Project>> GetRange(int left, int right)
        {
            return await _visitor.GetRange(left, right);
        }

        public async Task<IEnumerable<Entity.Product>> GetProductByProject(string projectTitle)
        {
            return await _visitor.GetProductByProject(projectTitle);
        }

        public async Task<bool> Add(Entity.Project project)
        {
            return await _changer.Add(project);
        }

        public async Task<bool> Delete(string projectTitle)
        {
            return await _changer.Delete(projectTitle);
        }

        public async Task<bool> Update(string oldProjectTitle, Project project)
        {
            return await _changer.Update(oldProjectTitle, project);
        }

        public async Task<bool> AddProducts(string projectTitle, HashSet<string> productTitles)
        {
            return await _changer.AddProducts(projectTitle, productTitles);
        }

        public async Task<bool> DeleteProducts(string projectTitle, HashSet<string> productTitles)
        {
            return await _changer.DeleteProducts(projectTitle, productTitles);
        }

        public async Task<bool> DeleteAllProduct(string projectTitle)
        {
            return await _changer.DeleteAllProduct(projectTitle);
        }
    }
}