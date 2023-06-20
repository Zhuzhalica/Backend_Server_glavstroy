using System.Collections.Generic;

namespace DbEntity
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Popularity { get; set; }
        public bool Available { get; set; }
        public string? PageLink { get; set; }
        public string? ImageRef { get; set; }
        
        public HeadingThree? HeadingThree { get; set; }
        public int? heading_three_id { get; set; }

        public ProductFamily ProductFamily { get; set; }
        public int product_family_id { get; set; }

        public List<SaleProducts> SaleProducts { get; set; }
        public List<ProductProperty> ProductProperties { get; set; }
        public List<ProjectMaterials> ProjectMaterials { get; set; }
        public List<Project> Projects { get; set; }

    }
}