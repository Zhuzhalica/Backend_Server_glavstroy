using Microsoft.EntityFrameworkCore;

namespace DbEntity
{
    public class ProjectMaterials
    {
        public int project_id { get; set; }
        public int product_id { get; set; }
        public Project Project { get; set; }
        public Product Product { get; set; }
    }
}