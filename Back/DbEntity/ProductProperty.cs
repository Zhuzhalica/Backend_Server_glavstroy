using Microsoft.EntityFrameworkCore;

namespace DbEntity
{
    public class ProductProperty
    {
        public int product_id { get; set; }
        public int property_values_id { get; set; }
        public Product Product{ get; set; }
        public PropertyValues PropertyValues { get; set; }

        public bool IsPriority { get; set; }
    }
}