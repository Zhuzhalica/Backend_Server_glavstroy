using Microsoft.EntityFrameworkCore;

namespace  DbEntity
{
    [Keyless]
    public class ProductProperty
    {
        public int product_id { get; set; }
        public int property_id { get; set; }
        public Product Product{ get; set; }
        public Property Property { get; set;}
        public string? PropertyValue { get; set;}
    }
}