using System.Threading.Tasks;
using Entity;

namespace BackServer.RepositoryChangers.Interfaces
{
    public interface IHeadersChanger
    {
        Task<bool> AddHeadingOne(Entity.HeadingOne headingOne);
        Task<bool> AddHeadingTwo(Entity.HeadingTwo headingTwo);
        Task<bool> AddHeadingThree(Entity.HeadingThree headingThree);
        Task<bool> DeleteHeadingOne(string title);
        Task<bool> DeleteHeadingTwo(string title);
        Task<bool> DeleteHeadingThree(string title);
        Task<bool> UpdateHeadingOne(string oldHeadingOneTitle, HeadingOne headingOne);
        Task<bool> UpdateHeadingTwo(string oldHeadingTwoTitle, HeadingTwo headingTwo);
        Task<bool> UpdateHeadingThree(string oldHeadingThreeTitle, HeadingThree headingThree);
    }
}