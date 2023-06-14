using System.Collections.Generic;

namespace DbEntity
{
    public class HeadingThree
    {
        public int Id { get; set; }
        public HeadingTwo HeadingTwo { get; set; }
        public Property Property { get; set; }
        public List<Product> Products { get; set; }
    }
}