using Microsoft.EntityFrameworkCore;

namespace DbEntity
{ 
    public class SaleProducts
    {
        public int product_id { get; set; }
        public int sale_id { get; set; }
        public Product Product { get; set; }
        public Sale Sale { get; set; }
    }
}