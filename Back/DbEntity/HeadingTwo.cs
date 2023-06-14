using System.Collections.Generic;

namespace DbEntity
{
    public class HeadingTwo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public HeadingOne HeadingOne { get; set; }
        public List<Product> Products { get; set; }
        public List<HeadingThree> HeadingsThree { get; set; }
    }
}