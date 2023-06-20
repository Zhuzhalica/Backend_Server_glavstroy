using System.Collections.Generic;

namespace DbEntity
{
    public class ProductFamily
    {
        public int Id { get; set; }
        public HeadingOne HeadingOne { get; set; }
        public HeadingTwo HeadingTwo { get; set; }
        public string Title { get; set; }
        public UnitMeasurement UnitMeasurement { get; set; }
        public List<Product> Products { get; set; }
    }
}