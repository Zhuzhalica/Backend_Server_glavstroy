using System.Collections.Generic;

namespace  DbEntity
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<ProductProperty> ProductProperties { get; set; }
        public List<HeadingThree> HeadingsThree { get; set; }
    }
}