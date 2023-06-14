using System.Collections.Generic;
using System.Threading.Tasks;
using BackServer.Repositories;
using BackServer.RepositoryChangers.Interfaces;
using BackServer.Services.Interfaces;
using Entity;

namespace BackServer.Services
{
    public class HeadersService : IHeadersService
    {
        private readonly IHeadersVisitor _visitor;
        private readonly IHeadersChanger _changer;

        public HeadersService(IHeadersVisitor visitor, IHeadersChanger changer)
        {
            _visitor = visitor;
            _changer = changer;
        }

        public async Task<IEnumerable<Entity.HeadingOne>> GetAllHeadingsOne()
        {
            return await _visitor.GetAllHeadingsOneAsync();
        }

        public async Task<IEnumerable<Entity.HeadingTwo>> GetAllHeadingsTwo()
        {
            return await _visitor.GetAllHeadingsTwoAsync();
        }

        public async Task<IEnumerable<Entity.HeadingTwo>> GetHeadingsTwoByHeadingsOne(string headingOneTitle)
        {
            return await _visitor.GetHeadingsTwoByHeadingsOneAsync(headingOneTitle);
        }

        public async Task<bool> AddHeadingOne(HeadingOne headingOne)
        {
            if (!CheckCorrectHeadingOne(headingOne))
                return false;
            
            return await _changer.AddHeadingOne(headingOne);
        }

        public async Task<bool> AddHeadingTwo(HeadingTwo headingTwo)
        {
            if (!CheckCorrectHeadingTwo(headingTwo))
                return false;
            
            return await _changer.AddHeadingTwo(headingTwo);
        }

        public async Task<bool> AddHeadingThree(HeadingThree headingThree)
        {
            if (!CheckCorrectHeadingThree(headingThree))
                return false;
            
            return await _changer.AddHeadingThree(headingThree);
        }

        public async Task<bool> DeleteHeadingOne(string title)
        {
            return await _changer.DeleteHeadingOne(title);
        }

        public async Task<bool> DeleteHeadingTwo(string title)
        {
            return await _changer.DeleteHeadingTwo(title);
        }

        public async Task<bool> DeleteHeadingThree(string title)
        {
            return await _changer.DeleteHeadingThree(title);
        }

        public async Task<bool> UpdateHeadingOne(string oldHeadingOneTitle, HeadingOne headingOne)
        {
            if (!CheckCorrectHeadingOne(headingOne))
                return false;

            return await _changer.UpdateHeadingOne(oldHeadingOneTitle, headingOne);
        }

        public async Task<bool> UpdateHeadingTwo(string oldHeadingTwoTitle, HeadingTwo headingTwo)
        {
            if (!CheckCorrectHeadingTwo(headingTwo))
                return false;

            return await _changer.UpdateHeadingTwo(oldHeadingTwoTitle, headingTwo);
        }

        public async Task<bool> UpdateHeadingThree(string oldHeadingThreeTitle, HeadingThree headingThree)
        {
            if (!CheckCorrectHeadingThree(headingThree))
                return false;

            return await _changer.UpdateHeadingThree(oldHeadingThreeTitle, headingThree);
        }

        private bool CheckCorrectHeadingOne(HeadingOne headingOne)
        {
            return headingOne.Title != null;
        }

        private bool CheckCorrectHeadingTwo(HeadingTwo headingTwo)
        {
            return headingTwo.Title != null && CheckCorrectHeadingOne(headingTwo.HeadingOne);
        }

        private bool CheckCorrectHeadingThree(HeadingThree headingThree)
        {
            return headingThree.Title != null && CheckCorrectHeadingTwo(headingThree.HeadingTwo);
        }
    }
}