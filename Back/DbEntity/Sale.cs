using System.Collections.Generic;

namespace DbEntity
{
    public class Sale
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Percent { get; set; }
        public List<SaleProducts> ProductsSales { get; set; }
    }
}