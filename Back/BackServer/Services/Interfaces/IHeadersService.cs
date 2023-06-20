using System.Collections.Generic;
using System.Threading.Tasks;
using BackServer.RepositoryChangers.Interfaces;
using Entity;

namespace BackServer.Services.Interfaces
{
    public interface IHeadersService
    {
        Task<IEnumerable<HeadingOne>> GetAllHeadingsOne();
        Task<IEnumerable<Entity.HeadingTwo>> GetAllHeadingsTwo();
        Task<IEnumerable<Entity.HeadingThree>> GetAllHeadingsThree();
        Task<IEnumerable<Entity.HeadingTwo>> GetHeadingsTwoByHeadingsOne(string headingOneTitle);
        Task<IEnumerable<Entity.HeadingThree>> GetHeadingsThreeByHeadingsTwoAsync(string headingTwoTitle);
        Task<bool> AddHeadingOne(HeadingOne headingOne);
        Task<bool> AddHeadingTwo(HeadingTwo headingTwo);
        Task<bool> AddHeadingThree(HeadingThree headingThree);
        Task<bool> DeleteHeadingOne(string title);
        Task<bool> DeleteHeadingTwo(string title);
        Task<bool> DeleteHeadingThree(string title);
        Task<bool> UpdateHeadingOne(string oldHeadingOneTitle, HeadingOne headingOne);
        Task<bool> UpdateHeadingTwo(string oldHeadingTwoTitle, HeadingTwo headingTwo);
        Task<bool> UpdateHeadingThree(string oldHeadingThreeTitle, HeadingThree headingThree);
    }
}