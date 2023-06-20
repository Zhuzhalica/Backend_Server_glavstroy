using System.Collections.Generic;

namespace DbEntity
{
    public class HeadingThree
    {
        public int Id { get; set; }
        public string? PageLink { get; set; }
        public string? ImageRef { get; set; }
        public HeadingTwo HeadingTwo { get; set; }
        public PropertyValues PropertyValues { get; set; }
        public List<Product> Products { get; set; }
    }
}