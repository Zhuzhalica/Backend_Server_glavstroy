using System.Collections.Generic;

namespace DbEntity
{
    public class PropertyValues
    {
        public int Id { get; set; }
        public Property Property { get; set;}
        public string PropertyValue { get; set; }
        public HeadingThree HeadingThree { get; set; }
        public List<ProductProperty> ProductProperties { get; set; }
    }
}