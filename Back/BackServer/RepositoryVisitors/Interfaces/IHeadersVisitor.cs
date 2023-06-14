using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace BackServer.Repositories
{
    public interface IHeadersVisitor
    {
        Task<IEnumerable<HeadingOne>> GetAllHeadingsOneAsync();
        Task<IEnumerable<HeadingTwo>> GetAllHeadingsTwoAsync();
        Task<IEnumerable<HeadingTwo>> GetHeadingsTwoByHeadingsOneAsync(string headingOneTitle);
    }
}